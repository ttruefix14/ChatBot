string path = "words.txt";
Tutor engTutor = new Tutor(path);

//engTutor.AddWord("dog", "собака");
//engTutor.AddWord("cat", "кошка");
//engTutor.AddWord("bird", "птица");
//engTutor.AddWord("fish", "рыба");

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