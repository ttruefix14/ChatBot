using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var nums = GetArrayFromFile("path.txt");
            foreach(var x in nums)
            {
                Console.WriteLine(x);
            }
        }

        static int[] GetArrayFromFile (string path)
        {
            try
            {
                string text = File.ReadAllText(path);
                var textNumbers = text.Split(' ');
                int[] numbers = new int[textNumbers.Length];
                for (int i = 0; i < textNumbers.Length; i++)
                {
                    numbers[i] = int.Parse(textNumbers[i]);
                }
                return numbers;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Не удалось считать содержимое файла");
                return new int[0];
            }

        }

        static int GetRowCount(string path)
        {
            try
            {
                var rows = File.ReadAllLines(path);
                return rows.Length;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Файл {path} не был найден!");
                return 0;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            catch (Exception ex)
            {   
                Console.WriteLine(ex.Message);
                var appEx = new AppException();
                appEx.MethodName = "GetRowCount";
                throw appEx;
            }
        }
    }
}
