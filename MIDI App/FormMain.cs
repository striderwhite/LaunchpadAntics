using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Threading;
using System.Runtime.InteropServices;

namespace MIDI_App
{

  public partial class FormMain : Form
  {
    enum ButtonColor : int { Green = 3 << 4, Red = 75, Orange = 23 };

    //Classes, containers...
    FormDrawer drawer = null;
    OutputDevice launchpadOut;
    InputDevice launchpadIn;
    Random rng = new Random();
    InputSpoofer inputSpoofer;

    //Low level stuff for midi
    ReadOnlyCollection<InputDevice> devicesCollection;
    List<InputDevice> devices;


    //CFG stuff..
    private const int SCREEN_WIDTH = 500;
    private const int SCREEN_HEIGHT = 500;
    private const int BPM = 250;
    private const int GRID_SIZE = 8;
    private const int green = 3 << 4;
    private const int red = 3;

    //For mapping walkthrough
    private const int AMOUNT_OF_PUBG_CONTROLS_TO_MAP = 15;
    private int CONTROL_MAPPING_STEP = 0;
    private bool IS_MAPPING_CONTROLLER = false;


    //TEST STUFF
    private int TestColor = 51;
    public int[,] grid = new int[GRID_SIZE, GRID_SIZE];

    //LOGICAL BOOL CONSTS 
    //Determines what input is passed
    bool PRESSED_W = false;
    bool PRESSED_A = false;
    bool PRESSED_S = false;
    bool PRESSED_D = false;
    bool PRESSED_SHIFT = false;
    bool PRESSED_UP = false;
    bool PRESSED_DOWN = false;
    bool PRESSED_LEFT = false;
    bool PRESSED_RIGHT = false;
    bool PRESSED_RCLICK = false;
    bool PRESSED_LCLICK = false; //(x == 6 && y == 2);  -- todo
    bool PRESSED_TAB = false;
    bool PRESSED_M = false;
    bool PRESSED_B = false;
    bool PRESSED_G = false;
    bool PRESSED_F = false;
    bool PRESSED_C = false;
    bool PRESSED_Z = false;
    bool PRESSED_SPACE = false;
    bool PRESSED_V = false;


    //DELEGATES
    //Delegate for midi button pressed
    delegate void DelMidiButtonPressed(Pitch note);
    DelMidiButtonPressed delMidiButtonPressed = MidiButtonPressed;

    private void FormMain_Load(object sender, EventArgs e)
    {
      //Visual stuff
      //BackColor = Color.FromArgb(255, 22, 30, 43);


      devicesCollection = InputDevice.InstalledDevices;
      devices = new List<InputDevice>(devicesCollection);
      inputSpoofer = new InputSpoofer();

      SendConsoleMessage("Attempting to open Launchpad...");

      launchpadIn = GetLaunchpadInputDevice();
      launchpadOut = GetLaunchpadOutputDevice();

      if (launchpadIn == null || launchpadOut == null)
      {
        MessageBox.Show("No Launchpad Detected - Restart application", "Error");
        SendConsoleMessage("Error: No Launchpad Detected - Restart application");
        labelStatus.Text = "Error: No Launchpad Detected - Restart application";
        labelStatus.BackColor = Color.LightPink;
        return;
      }

      SendConsoleMessage("Success!");
      labelStatus.Text = "Launchpad Connected!";
      labelStatus.ForeColor = Color.Green;

      launchpadIn.Open();
      launchpadOut.Open();
      launchpadIn.StartReceiving(new Clock(BPM));

      launchpadOut.SendControlChange(Channel.Channel1, (Midi.Control)0, 0);

      launchpadIn.NoteOn += Launchpad_NoteOn;
      launchpadIn.ControlChange += Launchpad_ControlChange;

      //Turn on approrate buttons for battlegrounds cnfg
      //W
      TurnOnButton(2, 1, ButtonColor.Green);
      //A
      TurnOnButton(1, 2, ButtonColor.Green);
      //S
      TurnOnButton(2, 2, ButtonColor.Green);
      //D
      TurnOnButton(3, 2, ButtonColor.Green);
      //SHIFT
      TurnOnButton(0, 3, ButtonColor.Green);
      //UP
      TurnOnButton(6, 1, ButtonColor.Orange);
      //DOWN
      TurnOnButton(6, 3, ButtonColor.Orange);
      //LEFT
      TurnOnButton(5, 2, ButtonColor.Orange);
      //RIGHT
      TurnOnButton(7, 2, ButtonColor.Orange);
      //RIGHT CLICK OR SHOOT
      TurnOnButton(6, 2, ButtonColor.Red);
      //TAB
      TurnOnButton(0, 5, ButtonColor.Orange);
      //M - MAP
      TurnOnButton(1, 5, ButtonColor.Orange);
      //B - Firemode
      TurnOnButton(7, 6, ButtonColor.Orange);
      //G - Swap Weapon
      TurnOnButton(6, 7, ButtonColor.Orange);
      //F - Action
      TurnOnButton(5, 1, ButtonColor.Green);
      //C - Crouch
      TurnOnButton(5, 3, ButtonColor.Green);
      //Z - Prone
      TurnOnButton(7, 3, ButtonColor.Green);
      //SPACE - Jump
      TurnOnButton(7, 1, ButtonColor.Green);
      //V - FPP toggle
      TurnOnButton(7, 7, ButtonColor.Green);
      //TurnOnAllButtons();

      Thread InputControllerThread = new Thread(OperateInput);
      InputControllerThread.Start();

    }


    //Primary operational thread of button spoofing
    private void OperateInput()
    {
      while (true)
      {

        //*****************************************
        //
        //      MOVE  ( W A S D SPACE)
        //
        //******************************************
        if (PRESSED_W)
        {
          inputSpoofer.SendKeys("w");
        }
        if (PRESSED_A)
        {
          inputSpoofer.SendKeys("a");
        }
        if (PRESSED_S)
        {
          inputSpoofer.SendKeys("s");
        }
        if (PRESSED_D)
        {
          inputSpoofer.SendKeys("d");
        }
        if (PRESSED_SPACE)
        {
          inputSpoofer.SendKeys("{SPACE}");
        }

        //*****************************************
        //
        //      MOVE MODIFIES ( C Z SHIFT )
        //
        //******************************************
        //CROUCH
        if (PRESSED_C)
        {
          inputSpoofer.SendKeys("c");
        }
        //PRONE
        if (PRESSED_Z)
        {
          inputSpoofer.SendKeys("z");
        }
        //SPRINT (HOLD)
        if (PRESSED_SHIFT)
        {
          inputSpoofer.SendKeys("{SHIFT}");
        }

        //*****************************************
        //
        //      LOOK  ( UP DOWN LEFT RIGHT )
        //
        //******************************************
        if (PRESSED_UP)
        {
          inputSpoofer.MoveMouse(0, 5);
        }
        if (PRESSED_DOWN)
        {
          inputSpoofer.MoveMouse(0, -5);
        }
        if (PRESSED_LEFT)
        {
          inputSpoofer.MoveMouse(-5, 0);
        }
        if (PRESSED_RIGHT)
        {
          inputSpoofer.MoveMouse(5, 0);
        }

        //*****************************************
        //
        //      SHOOT / AIM
        //
        //******************************************
        if (PRESSED_RCLICK)
        {

        }
        //TODO
        if (PRESSED_LCLICK)
        {

        }

        //*****************************************
        //
        //      INVENTORY, MAP, FPP TOGGLE, FIREMODE ( M TAB V B)
        //
        //******************************************
        //INV
        if (PRESSED_TAB)
        {
          inputSpoofer.SendKeys("{TAB}");
        }
        //MAP
        if (PRESSED_M)
        {
          inputSpoofer.SendKeys("m");
        }
        //V - FPP toggle
        if (PRESSED_V)
        {
          inputSpoofer.SendKeys("v");
        }
        //B - Firemode
        if (PRESSED_B)
        {
          inputSpoofer.SendKeys("b");
        }
        //G - Swap Weapon
        if (PRESSED_G)
        {
          inputSpoofer.SendKeys("g");
        }
        //F - Action
        if (PRESSED_F)
        {
          inputSpoofer.SendKeys("f");
        }


        Thread.Sleep(10);

      }
    }

    private void TurnOnAllButtons()
    {
      //HOW TO GET VALUE OF PITCH
      //(Pitch) = y * 16 + x
      for (int x = 0; x < GRID_SIZE; x++)
      {
        for (int y = 0; y < GRID_SIZE; y++)
        {

          //Convert from button prees coodinates to a 'pitch' struct
          int pitchNum = y * 16 + x;
          Pitch p = (Pitch)pitchNum;

          //draw colors
          switch (grid[x, y])
          {
            case 1:
              launchpadOut.SendNoteOn(Channel.Channel1, p, red);
              break;
            case 2:
              launchpadOut.SendNoteOn(Channel.Channel1, p, green);
              break;
            case 3:
              //launchpadOut.SendNoteOn(Channel.Channel1, p, rng.Next(1, 127));
              break;
          }
        }
      }
      return;

      //ignore below
      if (TestColor++ >= 127)
        TestColor = 75;
      foreach (Pitch p in Pitch.GetValues(typeof(Pitch)))
      {
        //launchpadOut.SendNoteOn(Channel.Channel1, p, rng.Next(1,127));
      }
    }

    private void TurnOffAllButtons()
    {
      foreach (Pitch p in Pitch.GetValues(typeof(Pitch)))
      {
        launchpadOut.SendNoteOff(Channel.Channel1, p, 3 << 4);
      }
    }

    private void TurnOnButton(Pitch p, ButtonColor c)
    {
      launchpadOut.SendNoteOn(Channel.Channel1, p, (int)c);
    }

    private void TurnOnButton(int x, int y, ButtonColor c)
    {
      //Convert into pitch
      int pitchNum = y * 16 + x;
      Pitch p = (Pitch)pitchNum;
      launchpadOut.SendNoteOn(Channel.Channel1, p, (int)c);
    }

    private void Launchpad_ControlChange(ControlChangeMessage msg)
    {
      throw new NotImplementedException();
    }


    //only called for mapping wizard
    private static void MidiButtonPressed(Pitch pitch)
    {
      Console.WriteLine("Got delegate message!");
      Console.WriteLine(pitch);
    }

    //On Button Press
    private void Launchpad_NoteOn(NoteOnMessage msg)
    {

      //If "configuring button mappings" flag == true
      //Jump into here and 'continue' the mapping process
      //Once the mapping process is completed, set flag off
      if (IS_MAPPING_CONTROLLER)
      {
        MidiButtonPressed(msg.Pitch);
      }

      //Convert from pitch to x.y
      int x = (int)msg.Pitch % 16;
      int y = (int)msg.Pitch / 16;

      SendConsoleMessage("Got X: " + x + " -- Y: " + y);

      //*****************************************
      //
      //      NOTE -- This sets all input to false if getting the button input again
      //      The reason for this is the launchpad sends another button press event
      //      when you let go of the button
      //
      //******************************************

      //*****************************************
      //
      //      MOVE  ( W A S D SPACE)
      //
      //******************************************
      if (PRESSED_W)
      {
        PRESSED_W = false;
        return;
      }
      if (PRESSED_A)
      {
        PRESSED_A = false;
        return;
      }
      if (PRESSED_S)
      {
        PRESSED_S = false;
        return;
      }
      if (PRESSED_D)
      {
        PRESSED_D = false;
        return;
      }
      if (PRESSED_SPACE)
      {
        PRESSED_SPACE = false;
        return;
      }

      //*****************************************
      //
      //      MOVE MODIFIES ( C Z SHIFT )
      //
      //******************************************
      //CROUCH
      if (PRESSED_C)
      {
        PRESSED_C = false;
        return;
      }
      //PRONE
      if (PRESSED_Z)
      {
        PRESSED_Z = false;
        return;
      }
      //SPRINT (HOLD)
      if (PRESSED_SHIFT)
      {
        PRESSED_SHIFT = false;
        return;
      }

      //*****************************************
      //
      //      LOOK  ( UP DOWN LEFT RIGHT )
      //
      //******************************************
      if (PRESSED_UP)
      {
        PRESSED_UP = false;
        return;
      }
      if (PRESSED_DOWN)
      {
        PRESSED_DOWN = false;
        return;
      }
      if (PRESSED_LEFT)
      {
        PRESSED_LEFT = false;
        return;
      }
      if (PRESSED_RIGHT)
      {
        PRESSED_RIGHT = false;
        return;
      }

      //*****************************************
      //
      //      SHOOT / AIM
      //
      //******************************************
      if (PRESSED_RCLICK)
      {
        PRESSED_RCLICK = false;
        return;
      }
      //TODO
      if (PRESSED_LCLICK)
      {
        PRESSED_LCLICK = false;
        return;
      }

      //*****************************************
      //
      //      INVENTORY, MAP, FPP TOGGLE, FIREMODE ( M TAB V B)
      //
      //******************************************
      //INV
      if (PRESSED_TAB)
      {
        PRESSED_TAB = false;
        return;
      }
      //MAP
      if (PRESSED_M)
      {
        PRESSED_M = false;
        return;
      }
      //V - FPP toggle
      if (PRESSED_V)
      {
        PRESSED_V = false;
        return;
      }
      //B - Firemode
      if (PRESSED_B)
      {
        PRESSED_B = false ;
        return;
      }
      //G - Swap Weapon
      if (PRESSED_G)
      {
        PRESSED_G = false;
        return;
      }
      //F - Action
      if (PRESSED_F)
      {
        PRESSED_F = false;
        return;
      }

      //LOGICAL BOOL CONSTS (set ON for what got pressed)
      PRESSED_W = (x == 2 && y == 1);
      PRESSED_A = (x == 1 && y == 2);
      PRESSED_S = (x == 2 && y == 2);
      PRESSED_D = (x == 3 && y == 2);
      PRESSED_SHIFT = (x == 0 && y == 3);
      PRESSED_UP = (x == 6 && y == 1);
      PRESSED_DOWN = (x == 6 && y == 3);
      PRESSED_LEFT = (x == 5 && y == 2);
      PRESSED_RIGHT = (x == 7 && y == 2);
      PRESSED_RCLICK = (x == 6 && y == 2);
      PRESSED_LCLICK = false; //(x == 6 && y == 2);  -- todo
      PRESSED_TAB = (x == 0 && y == 5);
      PRESSED_M = (x == 1 && y == 5);
      PRESSED_B = (x == 7 && y == 6);
      PRESSED_G = (x == 6 && y == 7);
      PRESSED_F = (x == 5 && y == 1);
      PRESSED_C = (x == 5 && y == 3);
      PRESSED_Z = (x == 7 && y == 3);
      PRESSED_SPACE = (x == 7 && y == 1);
      PRESSED_V = (x == 7 && y == 7);



      return;
      //Convert into pitch
      //int pitchNum = y * 16 + x;
      //Pitch p = (Pitch)pitchNum;
      //Console.WriteLine((int)msg.Pitch);
      //Console.WriteLine("x " + x + " y " + y);
      //Console.WriteLine(3 << 4);
      //launchpadOut.SendNoteOn(Channel.Channel1, msg.Pitch, ButtonColor.Orange);
      //Console.WriteLine(msg.Pitch);
    }

    private InputDevice GetLaunchpadInputDevice()
    {
      InputDevice device = null;
      devices.ForEach(x =>
      {
        if (x.Name == "Launchpad") { device = x; }
      });
      return device;
    }

    private OutputDevice GetLaunchpadOutputDevice()
    {

      List<OutputDevice> outdevices = new List<OutputDevice>(OutputDevice.InstalledDevices);
      OutputDevice device = null;
      outdevices.ForEach(x =>
      {
        if (x.Name == "Launchpad") { device = x; }
      });
      return device;
    }

    private void buttonToggleWalkthru_Click(object sender, EventArgs e)
    {
      Console.WriteLine(IS_MAPPING_CONTROLLER);
      //BEING MAPPING
      if (!IS_MAPPING_CONTROLLER)
      {
        SendConsoleMessage("Now mapping controls!");
        //toggle back
        IS_MAPPING_CONTROLLER = !IS_MAPPING_CONTROLLER;
        //STOP MAPPING
      }
      else
      {
        SendConsoleMessage("Stopped mapping controlls");
        //toggle back
        IS_MAPPING_CONTROLLER = !IS_MAPPING_CONTROLLER;
      }
    }

    private void SendConsoleMessage(string message)
    {
      dataGridViewConsole.Rows.Add(message, DateTime.Now.ToString());
      //data.Items.Add(new ListViewItem(new string[] { message , DateTime.Now.ToString() }));
    }

    private void dataGridViewConsole_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }


    public FormMain()
    {
      InitializeComponent();
      Paint += FormMain_Paint;
    }

    private void FormMain_Paint(object sender, PaintEventArgs e)
    {
      return;
      if (drawer == null)
      {
        drawer = new FormDrawer(e.Graphics, SCREEN_WIDTH, SCREEN_HEIGHT);
      }
      drawer.UpdateGraphics();

      grid = drawer.grid;
    }



  }
}
