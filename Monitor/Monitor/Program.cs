using System;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Reflection;

namespace Monitor
{
    class Program
    {
        private const UInt32 StdOutputHandle = 0xFFFFFFF5;
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
        [DllImport("kernel32.dll")]
        private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);
        [DllImport("kernel32")]
        static extern bool AllocConsole();
        public static void CreateConsole()
        {
            AllocConsole();

            // stdout's handle seems to always be equal to 7
            IntPtr defaultStdout = new IntPtr(7);
            IntPtr currentStdout = GetStdHandle(StdOutputHandle);

            if (currentStdout != defaultStdout)
                // reset stdout
                SetStdHandle(StdOutputHandle, defaultStdout);

            // reopen stdout
            TextWriter writer = new StreamWriter(Console.OpenStandardOutput())
            { AutoFlush = true };
            Console.SetOut(writer);
        }
        static void Main(string[] args)
        {
            CreateConsole();
            int hCount = int.Parse(args[0]);//int.Parse(args[0]);
            Console.Title = $"Длинна поля:{args[1]}";
            ProgressBarCreator pg = new ProgressBarCreator(hCount); //создание кол-ва прогрессбаров
            pg.CreateProgressBars();
            var mmf = MemoryMappedFile.CreateOrOpen("horseRide", hCount);
            var hrAccessor = mmf.CreateViewAccessor();
            List<sbyte> horsePoint = new List<sbyte>();
            List<int> summ = new List<int>();
            //Mutex mtx = Mutex.OpenExisting("horseMMF");
            //int temp = 1;
            while (true)
            {
                for (int i = 0; i < hCount; i++)
                {
                    sbyte temp = hrAccessor.ReadSByte(i);
                    pg.ProgressBarList[i].ProgressBarMove(temp);
                    Thread.Sleep(10);
                }
            }
            Console.ReadLine();
        }
    }
}
