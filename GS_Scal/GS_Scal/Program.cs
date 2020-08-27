using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace GS_Scal
{
    class Program
    {
        static SerialPort _serialPort;
        static int SW_HIDE = 0;
        //static int SW_SHOW = 5;
        private static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(),SW_HIDE);
            try
            {
                string pass = "hany";
                if (pass == args[3])
                {
                    string com = args[0];
                    int rate = int.Parse(args[1]);
                    int bit = int.Parse(args[2]);
                    _serialPort = new SerialPort(com,rate, Parity.Even, bit, StopBits.One);
                    _serialPort.Open();
                    string dataLine = _serialPort.ReadLine().Trim('S','s', ' ', 'g');
                    Clipboard.SetText(dataLine);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }
        }
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd , int nCmdShow);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

    }
}
