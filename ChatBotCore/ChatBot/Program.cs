﻿using System;
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
            string path = "words.txt";
            Tutor engTutor = new Tutor(path);

            string? word;
            string? translate;

            CheckWordResult result;
            while (true)
            {
                word = engTutor.GetRandomEngWord();
                if (word == null)
                {
                    Console.WriteLine("Словарь пуст");
                    break;
                }

                Console.Write($"Переведите слово {word}: ");
                translate = Console.ReadLine();
                if (translate == null)
                {
                    Console.WriteLine("Слова нет в словаре");
                    break;
                }
                result = engTutor.CheckWord(word, translate);

                switch (result)
                {
                    case CheckWordResult.Incorrect:
                        Console.WriteLine($"Неверно. Правильный ответ: \"{engTutor.Translate(word)}\"");
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