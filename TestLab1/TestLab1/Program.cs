using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;

namespace TestLab1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"C:\Git\Lab1MP\";
            string path = @"C:\Program Files (x86)\Labs\3_2sem\Lab1MP\";
            int horseCount;
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
            Console.WriteLine(Process.Start(path +  @"Monitor\Monitor\bin\Debug\Monitor.exe", monitor_args).Id);
            //Process.Start(@"C:\Git\Lab1MP\Horse\Horse\bin\Debug\Horse.exe", "1" + " " + horseCount.ToString() + " " + Process.GetCurrentProcess().Id);
            var mmfArbitr = MemoryMappedFile.CreateOrOpen("horseMMF", horseCount + 1);
            var arbitrAccessor = mmfArbitr.CreateViewAccessor(0, 0);
            arbitrAccessor.Write(0, (sbyte)horseCount);
            Semaphore ar = new Semaphore(0, horseCount+1, "ar");
            Mutex mtx = new Mutex(false, "mtx");
            List<int> ids = new List<int>();
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
