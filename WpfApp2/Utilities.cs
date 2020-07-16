using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp2
{
    class Utilities
    {
        public static Mutex thebigMUTEXboi = new Mutex();


        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }



        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENT_LEFTDOWN = 0x02;
        public const int MOUSEEVENT_LEFTUP = 0x04;


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);


        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);





        byte[] rgb;
        public Color GetColorAt(int x, int y)
        {
            IntPtr desk = GetDesktopWindow();
            IntPtr dc = GetWindowDC(desk);
            uint a = GetPixel(dc, x, y);
            ReleaseDC(desk, dc);
            //Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n{a}\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");

            //Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n\n\n\n R: {(byte)((a >> 0) & 0xff)}");
            //Console.WriteLine($"\n G: {(byte)((a >> 8) & 0xff)}\n");
            //Console.WriteLine($" B: {(byte)((a >> 16) & 0xff)}\n\n");

            //rgb = new byte[] { (byte)((a >> 0) & 0xff), (byte)((a >> 8) & 0xff), (byte)((a >> 16) & 0xff) };

            return Color.FromArgb(255, (byte)((a >> 0) & 0xff), (byte)((a >> 8) & 0xff), (byte)((a >> 16) & 0xff));
        }
        public static void leftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENT_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENT_LEFTUP, xpos, ypos, 0, 0);
        }
    }
}
