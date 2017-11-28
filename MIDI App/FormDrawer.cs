using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDI_App
{
  class FormDrawer
  {
    Graphics graphics = null;
    Random rng = new Random();
    Brush b = null;
    Brush bred = null;
    Brush byellow = null;
    Brush bgreen = null;
    Pen p = null;
    Color bgColor;
    private int SCREEN_WIDTH = 500;
    private int SCREEN_HEIGHT = 500;

    //test grid
    public int[,] grid = new int[8, 8];

    public FormDrawer(Graphics g, int width, int height)
    {
      SCREEN_WIDTH = width;
      SCREEN_HEIGHT = height;
      graphics = g;
      b = new SolidBrush(Color.Gray);
      bred = new SolidBrush(Color.LightSalmon);
      bgreen = new SolidBrush(Color.LightGreen);
      byellow = new SolidBrush(Color.LightYellow);

      p = new Pen(b);
      bgColor = Color.White;

      //gen some random numbers in there
      for (int x = 0; x < 8; x++)
      {
        for (int y = 0; y < 8; y++)
        {
          grid[x, y] = rng.Next(1, 4);
        }
      }
    }

    public void UpdateGraphics()
    {

      graphics.Clear(bgColor);
      int size = SCREEN_WIDTH / 8;

      for (int x = 0; x < 8; x++)
      {
        for (int y = 0; y < 8; y++)
        {
          //draw colors
          switch (grid[x, y])
          {
            case 1:
              graphics.FillRectangle(bred, new Rectangle(x * size, y * size, size, size));
              break;
            case 2:
              graphics.FillRectangle(bgreen, new Rectangle(x * size, y * size, size, size));
              break;
            case 3:
              graphics.FillRectangle(byellow, new Rectangle(x * size, y * size, size, size));
              break;
          }
          //draw grid
          graphics.DrawRectangle(p, new Rectangle(x * size, y * size, size, size));

        }
      }
    }

    public void DisposeTools()
    {
      graphics.Dispose();
      b.Dispose();
      p.Dispose();
    }
  }
}
