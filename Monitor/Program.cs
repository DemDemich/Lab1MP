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
            ProgressBarCreator pg = new ProgressBarCreator(4);
            pg.CreateProgressBars();
            pg.ProgressBarList[1].ProgressBarMove(2);
            pg.ProgressBarList[1].ProgressBarMove(3);
            /*var mmfHorse = MemoryMappedFile.CreateOrOpen("horseCount",3);
            var horseAccessor = mmfHorse.CreateViewAccessor();
            int horseCount = horseAccessor.ReadInt32(1);
            horseAccessor.Dispose();
            mmfHorse.Dispose();*/
            var mmf = MemoryMappedFile.CreateOrOpen("horseRide", 3);
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
