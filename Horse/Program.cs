using System;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Diagnostics;
namespace Horse
{
    class Program
    {
        static void Main(string[] args)
        {
            int horseCount = 3; //кол-во лошадей
            var mmfArbitr = MemoryMappedFile.CreateOrOpen("horseTest", horseCount + 1);
            var arbitrAccessor = mmfArbitr.CreateViewAccessor(0,0);
            arbitrAccessor.Write(0,(sbyte)horseCount);
            for (int i = 1; i < horseCount + 1; i++)
                arbitrAccessor.Write(i, (sbyte)i);
            
            Console.ReadLine();
        }
    }
}
