using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tutor engTutor = new Tutor();

            string word;
            string translate;
            engTutor.AddWord("dog", "собака");
            engTutor.AddWord("cat", "кошка");
            engTutor.AddWord("bird", "птица");
            engTutor.AddWord("whale", "кит");

            CheckWordResult result;
            while (true)
            {
                word = engTutor.GetRandomEngWord();

                Console.Write($"Переведите слово {word}: ");
                translate = Console.ReadLine();
                result = engTutor.CheckWord(word, translate);

                switch (result)
                {
                    case CheckWordResult.Incorrect:
                        Console.WriteLine($"Правильный ответ: \"{engTutor.Translate(word)}\"");
                        break;
                    case CheckWordResult.Correct:
                        Console.WriteLine("Верно!");
                        break;
                    case CheckWordResult.Unknown:
                        Console.WriteLine("Неизвестное слово");
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
