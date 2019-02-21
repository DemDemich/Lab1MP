using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
namespace Horse
{
    class Program
    {
        static void Main(string[] args)
        {
            var mmfHorse = MemoryMappedFile.CreateOrOpen("horseTest", 1); //файлик
            var horseAccessor = mmfHorse.CreateViewAccessor(); //писатель с глазами в файлик
            int Number = 0; //Номер лошадки
            bool end = false; //Флаг того дошла ли какая либо лошадка до конца
            int EndPos = 100; //Финальное расстояние
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
            ar.WaitOne();
            
            int i = 0; //Бег лошадки
            //ЛОШАДКА БЕЖИТ
            while (!end)
            {
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
                for (int j = 1; j <= C; j++) //Проверяем не добежал ли кто то до финиша
                {
                    if (horseAccessor.ReadSByte(i) >= EndPos)
                    {
                        Console.WriteLine("Кто то добежал до финиша раньше");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                horseAccessor.Write(Number, (sbyte)(horseAccessor.ReadSByte(Number) + 1));//Увеличиваем счетчик бега
                if (horseAccessor.ReadSByte(Number) >= EndPos)
                {
                    Console.WriteLine("Я на фишине");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }
    }
}
