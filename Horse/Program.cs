using System;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Text;
namespace Horse
{
    class Program
    {
        static void Main(string[] args)
        {
            var mmfHorse = MemoryMappedFile.CreateOrOpen("horseTest",50);
            var horseAccessor = mmfHorse.CreateViewAccessor();
            var h = "Horse1";
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(h);
            horseAccessor.WriteArray(0, bytes, 0, bytes.Length);
            //horseAccessor.WriteArray(h.Length+1,"Horse2",0,"Horse1".Length);
            bytes = new byte[h.Length];
            horseAccessor.ReadArray(0,bytes,0,bytes.Length);
            System.Console.WriteLine(ASCIIEncoding.ASCII.GetString(bytes));
            Console.ReadLine();
        }
    }
}
