using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;

namespace Horse
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Process.GetCurrentProcess().Id);
            foreach (var item in args)
            {
                Console.WriteLine(item.ToString());
            }
            Console.ReadKey();
            /*
            int Number = 0; //Номер лошадки
            //ОПРДЕЛЕНИЕ НОМЕРА
            Number = Convert.ToInt32(args[0]) - 1;
            //Кол-во лошадок
            int C = Convert.ToInt32(args[1]);
            var mmfHorse = MemoryMappedFile.CreateOrOpen("horseRide", C); //файлик
            var horseAccessor = mmfHorse.CreateViewAccessor(); //писатель с глазами в файлик
            
            bool end = false; //Флаг того дошла ли какая либо лошадка до конца
            int EndPos = 100; //Финальное расстояние
            

            //Semaphore ar = new Semaphore(0, 0); //Пустой семафор
            Semaphore ar = Semaphore.OpenExisting("ar");
            try
            {
                ar = Semaphore.OpenExisting("ar"); //Открываем семафор, что то надо с ним делать, хз как оно работает
                ar.WaitOne();
            }
            catch (WaitHandleCannotBeOpenedException) //Опа, а семафора то нет!
            {
                //КОНЕЦ ПРОГРАММЫ 
            }
            int i = 0; //Бег лошадки
            Mutex mtx = Mutex.OpenExisting("horseMMF");
            //ЛОШАДКА БЕЖИТ
            while (!end)
            {
                mtx.WaitOne();
                for (int j = 1; j <= C; j++) //Проверяем не добежал ли кто то до финиша
                {
                    if (horseAccessor.ReadSByte(i) >= EndPos)
                    {
                        //КОНЕЦ ПРОГРАММЫ
                    }
                }
                horseAccessor.Write(Number, (sbyte)(horseAccessor.ReadSByte(Number) + 1));//Увеличиваем счетчик бега
                if (horseAccessor.ReadSByte(Number) >= EndPos)
                {
                    //Конец забега - арбитр должен чекать как то 
                }
                mtx.ReleaseMutex();
            }*/
        }
    }
}
