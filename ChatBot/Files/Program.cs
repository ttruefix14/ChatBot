using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "text.txt";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Строка 1");
                sw.WriteLine("Строка 2");
            }

            if (File.Exists(path))
            {
                Console.WriteLine("Файл найден");
            }

            string line = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            var lines = File.ReadLines(path);
            foreach(var l in lines)
            {
                Console.WriteLine(l);
            }

            var text = File.ReadAllText(path);
            Console.WriteLine(text);
        }
    }
}
