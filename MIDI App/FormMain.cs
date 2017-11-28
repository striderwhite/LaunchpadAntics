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
    enum ButtonColor : int {Green = 3 << 4,Red = 75, Orange = 23};


    //Classes, containers...
    FormDrawer drawer = null;
    OutputDevice launchpadOut;
    InputDevice launchpadIn;
    Random rng = new Random();

    //Low level stuff for midi
    ReadOnlyCollection<InputDevice> devicesCollection;
    List<InputDevice> devices;


    //CFG stuff..
    private const int SCREEN_WIDTH = 500;
    private const int SCREEN_HEIGHT = 500;
    private const int BPM = 250;


    //For mapping walkthrough
    private const int AMOUNT_OF_PUBG_CONTROLS_TO_MAP = 15;
    private int CONTROL_MAPPING_STEP = 0;
    private bool IS_MAPPING_CONTROLLER = false;


    //TEST STUFF
    private int TestColor = 51;
    public int[,] grid = new int[8, 8];

    //DELEGATES

    //Delegate for midi button pressed
    delegate void DelMidiButtonPressed(Pitch note);
    DelMidiButtonPressed delMidiButtonPressed = MidiButtonPressed;

    public FormMain()
    {
      InitializeComponent();
      Paint += FormMain_Paint;
    }

    private void FormMain_Paint(object sender, PaintEventArgs e)
    {
      return;
      if (drawer == null){
        drawer = new FormDrawer(e.Graphics, SCREEN_WIDTH, SCREEN_HEIGHT);
      }
      drawer.UpdateGraphics();

      grid = drawer.grid;
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      //Visual stuff
      //BackColor = Color.FromArgb(255, 22, 30, 43);


      devicesCollection = InputDevice.InstalledDevices;
      devices = new List<InputDevice>(devicesCollection);

      SendConsoleMessage("Attempting to open Launchpad...");

      launchpadIn = GetLaunchpadInputDevice();
      launchpadOut = GetLaunchpadOutputDevice();

      if(launchpadIn == null || launchpadOut == null)
      {
        MessageBox.Show("No Launchpad Detected - Restart application", "Error");
        labelStatus.Text = "Error: No Launchpad Detected - Restart applicatio";
        labelStatus.BackColor = Color.Red;
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
      TurnOnButton(0,5, ButtonColor.Orange);
      //M - MAP
      TurnOnButton(1, 5, ButtonColor.Orange);
      //B - Firemode
      TurnOnButton(7,6, ButtonColor.Orange);
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

      Thread ButtonControllerThread = new Thread(OperateButtons);
      ButtonControllerThread.Start();

      

    }


    //Primary operational thread of lights
    private void OperateButtons()
    {
      while (true)
      {
        //TurnOnAllButtons();
        Thread.Sleep(1);
        //TurnOffAllButtons();
      }
    }

    private void TurnOnAllButtons()
    {
      //HOW TO GET VALUE OF PITCH
      //(Pitch) = y * 16 + x


      int green = 3 << 4;
      int red = 3;

      for (int x = 0; x < 8; x++)
      {
        for (int y = 0; y < 8; y++)
        {
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

      foreach (Pitch p in Pitch.GetValues(typeof(Pitch))) {

      




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
      launchpadOut.SendNoteOn(Channel.Channel1, p , (int)c);
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

      if(x == 6 && y == 1)
      {
        Win32.POINT mousePoint = new Win32.POINT();
        mousePoint.x = Cursor.Position.X;
        mousePoint.y = Cursor.Position.Y;
        mousePoint.x = mousePoint.x;
        mousePoint.x = mousePoint.y + 10;
        Win32.ClientToScreen(this.Handle, ref mousePoint);
        Win32.SetCursorPos(mousePoint.x, mousePoint.y);
      }


      //Convert into pitch
      int pitchNum = y * 16 + x;
      Pitch p = (Pitch)pitchNum;

 

      //Console.WriteLine((int)msg.Pitch);
      Console.WriteLine("x " + x + " y " + y);
      //Console.WriteLine(3 << 4);
      //launchpadOut.SendNoteOn(Channel.Channel1, msg.Pitch, ButtonColor.Orange);
      //Console.WriteLine(msg.Pitch);
    }

    private InputDevice GetLaunchpadInputDevice()
    {
      InputDevice device = null;
      devices.ForEach(x => {
        if(x.Name == "Launchpad"){device = x;}
      });
      return device;
    }

    private OutputDevice GetLaunchpadOutputDevice()
    {

      List<OutputDevice> outdevices = new List<OutputDevice>(OutputDevice.InstalledDevices);
      OutputDevice device = null;
      outdevices.ForEach(x => {
        if (x.Name == "Launchpad") { device = x; }
      });
      return device;
    }

    private void buttonToggleWalkthru_Click(object sender, EventArgs e)
    {
      Console.WriteLine(IS_MAPPING_CONTROLLER);
      //BEING MAPPING
      if (!IS_MAPPING_CONTROLLER){
        SendConsoleMessage("Now mapping controls!");
        //toggle back
        IS_MAPPING_CONTROLLER = !IS_MAPPING_CONTROLLER;
      //STOP MAPPING
      }else {
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
  }
}

public class Win32
{
  [DllImport("User32.Dll")]
  public static extern long SetCursorPos(int x, int y);

  [DllImport("User32.Dll")]
  public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

  [StructLayout(LayoutKind.Sequential)]
  public struct POINT
  {
    public int x;
    public int y;
  }
}
