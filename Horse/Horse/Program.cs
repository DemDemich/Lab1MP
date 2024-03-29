﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Horse
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
                    Environment.Exit(0);
                    return true;
                default:
                    return false;
            }
        }
        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(Handler, true);
            var mmfHorse = MemoryMappedFile.CreateOrOpen("horseMMF", int.Parse(args[1])); //файлик
            var horseAccessor = mmfHorse.CreateViewAccessor(); //писатель с глазами в файлик
            int Number = 0; //Номер лошадки
            bool end = false; //Флаг того дошла ли какая либо лошадка до конца
            int EndPos = 100; //Финальное расстояние
            Console.Title = args[0];
            //ОПРДЕЛЕНИЕ НОМЕРА
            try
            {
                Number = Convert.ToInt32(args[0]);
            }
            catch
            {
                Console.WriteLine("Номер лошади не передан!");
                Console.ReadLine();
                Environment.Exit(0);
            }
            //Кол-во лошадок
            int C = 0;
            try
            {
                C = Convert.ToInt32(args[1]);
            }
            catch
            {
                Console.WriteLine("Кол-во лошадей не передано!");
                Console.ReadLine();
                Environment.Exit(0);
            }
            //PID арбитр
            int ArbPID = 0;
            try
            {
                ArbPID = Convert.ToInt32(args[2]);
            }
            catch
            {
                Console.WriteLine("Арбитра нет!");
                Console.ReadLine();
                Environment.Exit(0);
            }
            Semaphore ar; //Пустой семафор
            if(!Semaphore.TryOpenExisting("ar",out ar))
            {
                Console.WriteLine("Семафора нет!");
                Console.ReadLine();
                Environment.Exit(0);
            }
            while (!ar.WaitOne())
            {
                try
                {
                    ArbPID = Convert.ToInt32(args[2]);
                    Process.GetProcessById(ArbPID);
                }
                catch
                {
                    Console.WriteLine("Арбитра нет!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            Mutex mtx;

            Mutex.TryOpenExisting("mtx",out mtx);


            int i = 0; //Бег лошадки
            //ЛОШАДКА БЕЖИТ

            //TODO: Сделать так чтобы лошадки не стопались когда приходит хотя бы 1 из них
            while (!end)
            {
                mtx.WaitOne();
                try
                {
                    ArbPID = Convert.ToInt32(args[2]);
                    Process.GetProcessById(ArbPID);
                }
                catch
                {
                    Console.WriteLine("Арбитра нет!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                /*for (int j = 1; j <= C; j++) //Проверяем не добежал ли кто то до финиша
                {
                    if (horseAccessor.ReadSByte(i) >= EndPos)
                    {
                        Console.WriteLine("Кто то добежал до финиша раньше");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }*/
                horseAccessor.Write(Number, (sbyte)(horseAccessor.ReadSByte(Number) + new Random().Next(0,10)));//Увеличиваем счетчик бега
                if (horseAccessor.ReadSByte(Number) >= EndPos)
                {
                    Console.WriteLine("Я на фишине");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                Thread.Sleep(new Random().Next(100, 500));
                mtx.ReleaseMutex();
            }
        }
    }
}
