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
    class Program
    {
        static void Main(string[] args)
        {

            ProgressBarCreator pg = new ProgressBarCreator(2); //создание кол-ва прогрессбаров
            pg.CreateProgressBars();
            //pg.ProgressBarList[1].ProgressBarMove(2);
            //pg.ProgressBarList[1].ProgressBarMove(3);
            var mmf = MemoryMappedFile.CreateOrOpen("horseRide", 3);
            var hrAccessor = mmf.CreateViewAccessor();
            List<sbyte> horsePoint = new List<sbyte>();
            for (int i = 2; i < 100; i++)
            {
                pg.ProgressBarList[1].ProgressBarMove(i);
                Thread.Sleep(100);
            }
            /* while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    horsePoint.Add(hrAccessor.ReadSByte(i));
                }
                break;
            }*/
            Console.ReadLine();
        }
    }
}
