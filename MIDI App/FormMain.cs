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

namespace MIDI_App
{

  public partial class FormMain : Form
  {
    OutputDevice launchpadOut;
    InputDevice launchpadIn;

    Random rng = new Random();

    ReadOnlyCollection<InputDevice> devicesCollection;
    List<InputDevice> devices;

    private readonly int BPM = 250;
    private int TestColor = 51;

    public FormMain()
    {
      InitializeComponent();
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      devicesCollection = InputDevice.InstalledDevices;
      devices = new List<InputDevice>(devicesCollection);

      launchpadIn = GetLaunchpadInputDevice();
      launchpadOut = GetLaunchpadOutputDevice();

      launchpadIn.Open();
      launchpadOut.Open();
      launchpadIn.StartReceiving(new Clock(BPM));

      launchpadOut.SendControlChange(Channel.Channel1, (Midi.Control)0, 0);

      launchpadIn.NoteOn += Launchpad_NoteOn;
      launchpadIn.ControlChange += Launchpad_ControlChange;

      TurnOnAllButtons();

      Thread ButtonControllerThread = new Thread(operateButtons);
      ButtonControllerThread.Start();

    }

    private void operateButtons()
    {
      while (true)
      {
        TurnOnAllButtons();
        Thread.Sleep(1);
        TurnOffAllButtons();
      }
    }

    private void TurnOnAllButtons()
    {
      if (TestColor++ >= 127)
        TestColor = 75;

      foreach (Pitch p in Pitch.GetValues(typeof(Pitch))) {

        int green = 3 << 4;
        int red = 3;

        launchpadOut.SendNoteOn(Channel.Channel1, p, rng.Next(1,127));
      }
    }

    private void TurnOffAllButtons()
    {
      foreach (Pitch p in Pitch.GetValues(typeof(Pitch)))
      {
        launchpadOut.SendNoteOff(Channel.Channel1, p, 3 << 4);
      }
    }

    private void Launchpad_ControlChange(ControlChangeMessage msg)
    {
      throw new NotImplementedException();
    }

    private void Launchpad_NoteOn(NoteOnMessage msg)
    {
      //Bottom left = (0,0)
      //Top right = (7,7)
      int x = (int)msg.Pitch % 16;
      int y = (int)msg.Pitch / 16;

      //Console.WriteLine((int)msg.Pitch);
      Console.WriteLine("x " + x + " y " + y);
      Console.WriteLine(3 << 4);
      launchpadOut.SendNoteOn(Channel.Channel1, msg.Pitch, 3 << 4);

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

  }
}
