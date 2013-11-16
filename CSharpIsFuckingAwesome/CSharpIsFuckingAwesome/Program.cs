using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Forms;


namespace CSharpIsFuckingAwesome
{

    public class MouseSimulator
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }
        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
        enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }

        public static void LeftMouseButtonDown()
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
        }

        public static void LeftMouseButtonUp()
        {
            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
        }

        public static void ClickLeftMouseButton()
        {
            LeftMouseButtonDown();
            LeftMouseButtonUp();
        }

        public static void RightMouseButtonDown()
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
        }

        public static void RightMouseButtonUp()
        {
            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
        }

        public static void ClickRightMouseButton()
        {
            RightMouseButtonDown();
            RightMouseButtonUp();            
        }

        public static void LinearSmoothMove(Point newPosition, int steps)
        {
            Point start = Cursor.Position;
            PointF iterPoint = start;

            // Find the slope of the line segment defined by start and newPosition
            PointF slope = new PointF(newPosition.X - start.X, newPosition.Y - start.Y);

            // Divide by the number of steps
            slope.X = slope.X / steps;
            slope.Y = slope.Y / steps;

            // Move the mouse to each iterative point.
            for (int i = 0; i < steps; i++)
            {
                iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                SetCursorPos(Point.Round(iterPoint).X, Point.Round(iterPoint).Y);
                Thread.Sleep(10);
            }

            // Move the mouse to the final destination.
            SetCursorPos(newPosition.X, newPosition.Y);
        }
    }

    class Program
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static int Height = Screen.PrimaryScreen.Bounds.Height;
        public static int Width = Screen.PrimaryScreen.Bounds.Width;
        
        static void Main(string[] args)
        {
            //Console.WriteLine("Height: " + Height + " Width: " + Width);
            bool firstTime = true;
            //System.out.println()
            Console.WriteLine("Hello World!");

            System.IO.Ports.SerialPort p = new SerialPort("COM3");
          
           p.Open();

           char[] input = new char[10];
           int average = 0;
           int counter = 0;
           int[] values = new int[10];
           char previous = 'k';
           int previousAverage = 0;
           int almightyCounter = 0;

           SetCursorPos(Width / 2, Height / 2);
           
           while (true)
           {
               
               
               //Point position = Cursor.Position;
       
               String s = p.ReadLine();
              //s = s.Replace("/r", "");

              //average = Convert.ToInt32(s);

              input = s.ToCharArray();


             // if (s.ToCharArray()[0] == '0')
             // {
             //     Console.WriteLine("Yes");
             // }


              // Console.WriteLine("X: " + position.X + " Y: " + position.Y);
          
              Console.WriteLine(s);
               //Console.WriteLine(s.Length);

              if (s.Length == 0)
              {

              }
              else if (s.Length - 1 == 0)
              {

              }
              else if (s == "\r")
              {

              }
              else if (s[0] == 'C')
              {

                  if (input[1] == '0' && input[1] != previous)
                  {
                      if (previous == '1')
                      {
                          MouseSimulator.LeftMouseButtonUp();
                      }

                      if (previous == '2')
                      {
                          MouseSimulator.RightMouseButtonUp();
                      }
                      

                      //mouse_event(MOUSEEVENTF_RIGHTUP, position.X, position.Y, 0, 0);
                      //mouse_event(MOUSEEVENTF_LEFTUP, position.X, position.Y, 0, 0);
                  }

                  if (input[1] == '1' && input[1] != previous)
                  {

                      //MouseSimulator.ClickLeftMouseButton();
                      //Thread.Sleep(100);
                      //MouseSimulator.RightMouseButtonUp();
                      MouseSimulator.LeftMouseButtonDown();
                      //mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                      //mouse_event(0x02, position.X, position.Y, 0, 0);
                      //Thread.Sleep(100);
                  }

                  if (input[1] == '2' && input[1] != previous)
                  {
                      //MouseSimulator.ClickRightMouseButton();
                      //Thread.Sleep(100);
                      //MouseSimulator.LeftMouseButtonUp();
                      MouseSimulator.RightMouseButtonDown();
                      //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                      //mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);

                  }
                  previous = input[1];
              }
              else
              {
                  if (s[0] == 'L')
                  {
                      String value;
                      if (s.Length - 1 == 6)
                      {
                          value = String.Concat(s[2] + s[3] + s[4] + s[5]);
                      }
                      else if (s.Length - 1 == 5)
                      {
                          value = String.Concat(s[2] + s[3] + s[4]);
                      }
                      else if (s.Length - 1 == 4)
                      {
                          value = String.Concat(s[2] + s[3]);
                      }
                      else
                      {
                          value = String.Concat(s[2]);
                      }

                      int number = Convert.ToInt32(value);
                      number /= 10;

                      Point position = Cursor.Position;
                      //MouseSimulator.LinearSmoothMove(new Point(position.X + number, position.Y), 10);
                      SetCursorPos(position.X + number, position.Y);
                  }
                  else if (s[0] == 'R')
                  {
                      String value;
                      if (s.Length - 1 == 6)
                      {
                          value = String.Concat(s[2] + s[3] + s[4] + s[5]);
                      }
                      else if (s.Length - 1 == 5)
                      {
                          value = String.Concat(s[2] + s[3] + s[4]);
                      }
                      else if (s.Length - 1 == 4)
                      {
                          value = String.Concat(s[2] + s[3]);
                      }
                      else
                      {
                          value = String.Concat(s[2]);
                      }

                      int number = Convert.ToInt32(value);
                      number /= 10;

                      Point position = Cursor.Position;

                      //MouseSimulator.LinearSmoothMove(new Point(position.X - number, position.Y), 10);
                      SetCursorPos(position.X - number, position.Y);
                  }
                  else if (s[0] == 'S')
                  {
                      //Nothing
                  }
                  else if (s[0] == 'D')
                  {
                      String value;
                      if (s.Length - 1 == 6)
                      {
                          value = String.Concat(s[2] + s[3] + s[4] + s[5]);
                      }
                      else if (s.Length - 1 == 5)
                      {
                          value = String.Concat(s[2] + s[3] + s[4]);
                      }
                      else if (s.Length - 1 == 4)
                      {
                          value = String.Concat(s[2] + s[3]);
                      }
                      else
                      {
                          value = String.Concat(s[2]);
                      }

                      int number = Convert.ToInt32(value);
                      number /= 10;

                      Point position = Cursor.Position;

                      //MouseSimulator.LinearSmoothMove(new Point(position.X, position.Y + number), 10);
                      SetCursorPos(position.X, position.Y + number);
                  }
                  else if (s[0] == 'U')
                  {
                      String value;
                      if (s.Length - 1 == 6)
                      {
                          value = String.Concat(s[2] + s[3] + s[4] + s[5]);
                      }
                      else if (s.Length - 1 == 5)
                      {
                          value = String.Concat(s[2] + s[3] + s[4]);
                      }
                      else if (s.Length - 1 == 4)
                      {
                          value = String.Concat(s[2] + s[3]);
                      }
                      else
                      {
                          value = String.Concat(s[2]);
                      }

                      int number = Convert.ToInt32(value);
                      number /= 10;

                      Point position = Cursor.Position;

                      //MouseSimulator.LinearSmoothMove(new Point(position.X, position.Y - number), 10);
                      SetCursorPos(position.X, position.Y - number);
                  }







              }

                     // values[i] = Convert.ToInt32(number);
                 // }

                 /* for (int i = 0; i < 10; i++)
                  {
                      average += values[i];
                  }
                  average /= 2;

                  for (int i = 0; i < 10; i++)
                  {
                      if (values[i] <= average - 2250)
                      {
                          //mouse left
                          Point position = Cursor.Position;
                          SetCursorPos(position.X - 1, position.Y);
                      }
                      else if (values[i] >= average + 2250)
                      {
                          //mouse right
                          Point position = Cursor.Position;
                          SetCursorPos(position.X + 1, position.Y);
                      }
                  }
                  */


              }

               
               //Thread.Sleep(100);
           }
       

           /* Console.WriteLine("Moving mouse");

            for (int i = 100; i < 150; i++)
            {
                SetCursorPos(i, i);
                mouse_event(0x02, i, i, 0, 0);
                Thread.Sleep(100);
            }
            int daniel = 't';
            
            while(true)
            {
                if(Console.Read() == 'k')
                {
                    daniel = 'k';
                } else if(Console.Read() == 'o')
                {
                    daniel = 'o';
                }

                if(daniel == 'k')
                {

                }

                mouse_event(0x02, 
            }

            int j = 10;
            myFunction(out j);
            //j == 5
            Console.WriteLine(j.ToString());

            

            */
           // Console.WriteLine("done");
            //Console.Read();
        

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        
     
    }
}
