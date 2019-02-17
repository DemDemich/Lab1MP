using System;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Reflection;

namespace Monitor
{
    public enum CharacterAttributes
    {
        FOREGROUND_BLUE = 0x0001,
        FOREGROUND_GREEN = 0x0002,
        FOREGROUND_RED = 0x0004,
        FOREGROUND_INTENSITY = 0x0008,
        BACKGROUND_BLUE = 0x0010,
        BACKGROUND_GREEN = 0x0020,
        BACKGROUND_RED = 0x0040,
        BACKGROUND_INTENSITY = 0x0080,
        COMMON_LVB_LEADING_BYTE = 0x0100,
        COMMON_LVB_TRAILING_BYTE = 0x0200,
        COMMON_LVB_GRID_HORIZONTAL = 0x0400,
        COMMON_LVB_GRID_LVERTICAL = 0x0800,
        COMMON_LVB_GRID_RVERTICAL = 0x1000,
        COMMON_LVB_REVERSE_VIDEO = 0x4000,
        COMMON_LVB_UNDERSCORE = 0x8000
    }
    class Program
    {
        [DllImport("kernel32.dll")] 
        public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, CharacterAttributes wAttributes);
        static void Main(string[] args)
        {
            IntPtr hOut = GetStdHandle(-11);
            SetConsoleTextAttribute(hOut, CharacterAttributes.BACKGROUND_GREEN);
            Console.SetCursorPosition(1,1);
            Console.Write(" ");
            Process[] allP = Process.GetProcesses();
            List<Process> horseList = new List<Process>();
            
            foreach (var item in allP)
            {
                if(item.MainWindowTitle.Contains("Horse"))
                    horseList.Add(item);
            }
            var mmf = MemoryMappedFile.CreateOrOpen("horseRide",3);
            var hrAccessor = mmf.CreateViewAccessor();
            List<int> horsePoint = new List<int>();
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    horsePoint.Add(hrAccessor.ReadInt32(i));
                }
                break;
            }  

            Console.ReadLine();
        }
    }
}
