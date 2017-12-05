using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace MIDI_App
{
  class InputSpoofer
  {
    //===========================
    //        MOUSE
    //===========================
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
    private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
    private const uint MOUSEEVENTF_LEFTUP = 0x04;
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const uint MOUSEEVENTF_RIGHTUP = 0x10;


    //moves mouse by x/y amount from current pos
    public void MoveMouse(int x, int y)
    {
      Cursor.Position = new System.Drawing.Point(Cursor.Position.X + x, Cursor.Position.Y + y);
    }

    public void SendKeys(string key)
    {
      System.Windows.Forms.SendKeys.Send(key);
    }
  }
}
