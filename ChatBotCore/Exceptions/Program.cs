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
            string path = "jgasdk/asdgalk";
            try
            {
                var v = GetRowCount(path);
                Console.WriteLine(v);
            }
            catch (AppException e)
            {
                Console.WriteLine(e.MethodName);
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
