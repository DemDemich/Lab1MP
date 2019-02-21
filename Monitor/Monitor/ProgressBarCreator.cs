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
    public class ProgressBarCreator
    {
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, CharacterAttributes wAttributes);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputCharacter(IntPtr hConsoleOutput,
                                  string lpCharacter, uint nLength, COORD dwWriteCoord,
                                                                  out uint lpNumberOfCharsWritten);
        List<ConsoleProgressBar> l_cpg = new List<ConsoleProgressBar>();
        int i_posTop = 0;
        public ProgressBarCreator(int in_count)
        {
            for (int i = 0; i < in_count; i++)
            {
                l_cpg.Add(new ConsoleProgressBar(i_posTop));
                i_posTop += 2;
            }
        }
        public List<ConsoleProgressBar> ProgressBarList
        {
            get => l_cpg;
        }
        public void CreateProgressBars()
        {
            int i = 1;
            uint t = 0;
            foreach (var item in l_cpg)
            {
                SetConsoleTextAttribute(item.ConsoleHandle, CharacterAttributes.FOREGROUND_GREEN);
                WriteConsoleOutputCharacter(item.ConsoleHandle, $"Horse{i++}", 6,item.cords,out t);
                item.cords.Y++;
                WriteConsoleOutputCharacter(item.ConsoleHandle, "[", 1, item.cords, out t);
                item.cords.X += (short)item.MaxLeftPos;
                WriteConsoleOutputCharacter(item.ConsoleHandle, "]", 1, item.cords, out t);
                item.cords.X = 1;
            }
        }
    }
}