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
            int hCount = 2;//int.Parse(args[0]);
            //Console.Title = $"Длинна поля:{args[1]}";
            ProgressBarCreator pg = new ProgressBarCreator(hCount); //создание кол-ва прогрессбаров
            pg.CreateProgressBars();
            var mmf = MemoryMappedFile.CreateOrOpen("horseRide", hCount);
            var hrAccessor = mmf.CreateViewAccessor();
            List<sbyte> horsePoint = new List<sbyte>();
            List<int> summ = new List<int>();
            Mutex mtx = Mutex.OpenExisting("horseMMF");
            while (true)
            {
                for (int i = 0; i < hCount; i++)
                {
                    sbyte temp = hrAccessor.ReadSByte(i);
                    pg.ProgressBarList[i].ProgressBarMove(temp);
                    //horsePoint.Add(temp);
                }
            }
            Console.ReadLine();
        }
    }
}
