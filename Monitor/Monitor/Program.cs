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

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);
        private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        public static List<int> ids;
        public static int moni;
        /// <summary>
        /// Для котроля нажания CTRL+SMTH
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private static bool Handler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_BREAK_EVENT:
                    Console.WriteLine("CTRL+BREAK");
                    return true;
                case CtrlType.CTRL_C_EVENT:
                    Console.WriteLine("CTRL+C");
                    return true;
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    Console.WriteLine("Closing");
                    Environment.Exit(0);
                    return true;
                default:
                    return false;
            }
        }
        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(Handler, true);
            int hCount = int.Parse(args[0]);
            Console.Title = $"Длинна поля:{args[1]}";
            ProgressBarCreator pg = new ProgressBarCreator(hCount); //создание кол-ва прогрессбаров
            pg.CreateProgressBars();
            var mmf = MemoryMappedFile.CreateOrOpen("horseMMF", hCount);
            var hrAccessor = mmf.CreateViewAccessor();
            List<sbyte> horsePoint = new List<sbyte>();
            List<int> summ = new List<int>();
            Semaphore ar;
            Semaphore.TryOpenExisting("ar", out ar);
            sbyte[] hrMoves = new sbyte[hCount];
            ar.WaitOne();
            //TODO: пофиксить баг с отображением 1+ прогрессбара
            //Улучшено считывание с memory mapped file
            while (true)
            {
                hrAccessor.ReadArray<sbyte>(0, hrMoves, 0, hCount);
                for (int i = 0; i < hCount; i++)
                {
                    pg.ProgressBarList[i].ProgressBarMove(hrMoves[i]);
                }
            }
            Console.ReadLine();
        }
    }
}
