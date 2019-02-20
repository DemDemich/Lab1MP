using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;

namespace arbiter
{
    class Program
    {
        static void Main(string[] args)
        {
            int horseCount;
            while (true)
            try
            {
                Console.Write("Введите количество участников - ");
                horseCount = int.Parse(Console.ReadLine());
                Console.Write("Введите длину поля - ");
                int len = int.Parse(Console.ReadLine());
                break;
            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка!");
            }
            //Console.WriteLine(Process.Start(@"D:\Учеба\test\test\bin\Debug\test.exe").Id);
            //Console.WriteLine(Process.Start(@"monitor.exe",horseCount.toSring(),len.toSring()).Id); // запуск монитора и передача ему начальных аргументов
            var mmfArbitr = MemoryMappedFile.CreateOrOpen("horseTest", horseCount + 1);
            var arbitrAccessor = mmfArbitr.CreateViewAccessor(0, 0);
            arbitrAccessor.Write(0, (sbyte)horseCount);            
            Semaphore ar = new Semaphore(horseCount, horseCount, "ar");
            List<int> ids = new List<int>();
            for (int i = 0; i < horseCount; i++)
            {
                //ids.Add(Process.Start(@"D:\Учеба\test\test\bin\Debug\test.exe").Id);
                Console.WriteLine("HORSE - {0}",ids[i]);
                //Console.WriteLine(Process.Start(@"horse.exe",i.toString(),horseCount.toSring()).Id); //создание лошади и передача ему его номера
            }
            while (true)
                try
                {
                    Console.Write("чтобы начать гонку введите 1 - ");
                    Console.Write("чтобы завершить гонку нажмите 0 -    ");
                    int d = int.Parse(Console.ReadLine());
                    if (d== 1)
                    {
                        ar.Release();
                        break;
                    }
                    else if(d == 0)
                    {
                        foreach (var item in ids)
                        {
                            Process.GetProcessById(item).Kill();
                            
                        }
                        break;
                    }                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка!");

                }
            Process.GetCurrentProcess().Close();
            Console.WriteLine(Process.GetCurrentProcess().Id);
            Console.ReadKey();
        }
    }
}
