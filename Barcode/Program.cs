using System;
using System.IO;

namespace Barcode
{
    class Program
    {
        static void Main(string[] args)
        {
            Barcode39 barcode = new Barcode39();
            Console.Write("Enter your barcode: ");
            var input = Console.ReadLine();
            string result = barcode.DrawCode39Barcode(input, 0);
            File.WriteAllText("barcode.html", result);
        }
    }
}
