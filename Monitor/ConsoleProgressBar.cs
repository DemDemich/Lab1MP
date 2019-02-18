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
    public class ConsoleProgressBar
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, CharacterAttributes wAttributes);
        int i_currentPosTop;
        int i_currentPosLeft;
        IntPtr hOut;
        const int i_max = 101;
        public ConsoleProgressBar()
        {
            ConsoleHandle = GetStdHandle(-11);
        }
        public ConsoleProgressBar(int topPos)
        {
            ConsoleHandle = GetStdHandle(-11);
            CurrentPosTop = topPos;
            CurrentPosLeft = 0;
        }
        #region Properties
        public int CurrentPosTop
        {
            get => i_currentPosTop;
            set => i_currentPosTop = value;
        }
        public int CurrentPosLeft
        {
            get => i_currentPosLeft;
            set => i_currentPosLeft= value;
        }
        public IntPtr ConsoleHandle
        {
            get => hOut;
            set => hOut = value;
        }
        public int MaxLeftPos
        {
            get => i_max;
        }
        #endregion
        public void ProgressBarMove(int newPosLeft)
        {
            SetConsoleTextAttribute(ConsoleHandle, CharacterAttributes.FOREGROUND_GREEN);
            CurrentPosLeft += (newPosLeft - CurrentPosLeft) - 1;
            Console.SetCursorPosition(CurrentPosLeft, CurrentPosTop+1);
            Console.Write("#");
        }
    }
}