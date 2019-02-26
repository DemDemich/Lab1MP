using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
namespace TestLab1
{
    class Program
    {
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
                    foreach (var item in ids)
                    {
                        Process.GetProcessById(item).Kill();
                    }
                    Process.GetProcessById(moni).Kill();
                    Environment.Exit(0);
                    return true;
                default:
                    return false;
            }
        }
        static void Main(string[] args)
        {
            
            string path = @"D:\LabsUniver\3_2kurs\Lab1MP\";
            //string path = @"C:\Git\Lab1MP\";
            int horseCount;
            SetConsoleCtrlHandler(Handler, true);
            int len;
            while (true)
                try
                {
                    Console.Write("Введите количество участников - ");
                    horseCount = int.Parse(Console.ReadLine());
                    Console.Write("Введите длину поля - ");
                    len = int.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка!");
                }
            string monitor_args = horseCount.ToString() + " " + len.ToString();
            moni = Process.Start(path + @"Monitor\Monitor\bin\Debug\Monitor.exe", monitor_args).Id;
            Console.WriteLine(moni);
            var mmfArbitr = MemoryMappedFile.CreateOrOpen("horseMMF", horseCount + 1);
            var arbitrAccessor = mmfArbitr.CreateViewAccessor(0, 0);
            arbitrAccessor.Write(0, (sbyte)horseCount);
            Semaphore ar = new Semaphore(0, horseCount+1, "ar");
            Mutex mtx = new Mutex(false, "mtx");
            ids = new List<int>();
            for (int i = 0; i < horseCount; i++) //запускаем лошадей
            {
                ids.Add(Process.Start(path +  @"Horse\Horse\bin\Debug\Horse.exe", i.ToString() + " " + horseCount.ToString() + " " + Process.GetCurrentProcess().Id).Id);//создание лошади и передача ему его номера
                //Console.WriteLine("HORSE - {0}", ids[i]);
                //Console.WriteLine(Process.Start(@"horse.exe",i.toString(),horseCount.toSring()).Id);
            }
            while (true)
                try
                {
                    Console.Write("Чтобы начать гонку введите 1 - ");
                    Console.Write("Чтобы завершить гонку нажмите 0 - ");
                    int d = int.Parse(Console.ReadLine());
                    if (d == 1)
                    {
                        ar.Release(horseCount+1);
                        break;
                    }
                    else if (d == 0)
                    {
                        foreach (var item in ids)
                        {   
                            Process.GetProcessById(item).Kill();

                        }
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Ошибка!");

                }
            Process.GetCurrentProcess().Close();
            Console.WriteLine(Process.GetCurrentProcess().Id);
            
            Console.ReadKey();
        }
    }
}
