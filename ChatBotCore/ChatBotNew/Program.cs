using Telegram;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

const string COMMAND_LIST = @"Список команд:
/add <eng> <rus> - добавление английского слова и его перевод в словарь
/get - получаем случайное английское слово из словаря
/check <eng> <rus> - проверяем правильность перевода английского слова
";

string path = "words.txt";
Tutor engTutor = new Tutor(path);

TelegramBotClient botClient = new TelegramBotClient(args[0]);

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var username = message.Chat.Username;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    var msgArgs = message.Text.Split(' ');

    Message sentMessage;
    switch (msgArgs[0])
    {
        case "/help":
            sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: COMMAND_LIST,
                cancellationToken: cancellationToken);
            break;
        case "/add":
            if (msgArgs.Length < 3) 
            {
                Console.WriteLine("Неверное количество аргументов");
                return; 
            }
            engTutor.AddWord(msgArgs[1], msgArgs[2]);
            break;
        case "/get":
            var randomWord = engTutor.GetRandomEngWord() ?? "Слово отсутствует в словаре";
            
            sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: randomWord,
                cancellationToken: cancellationToken);
            break;
        case "/check":
            if (msgArgs.Length < 3)
            {
                Console.WriteLine("Неверное количество аргументов");
                return;
            }
            var result = engTutor.CheckWord(msgArgs[1], msgArgs[2]) switch
            {
                CheckWordResult.Unknown => $"Слово \"{msgArgs[1]}\" отсутствует в словаре",
                CheckWordResult.Incorrect => $"Правильный ответ: \"{engTutor.Translate(msgArgs[1])}\"",
                CheckWordResult.Correct => "Верно!",
                _ => "Неизвестно"
            };
            sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: result,
                cancellationToken: cancellationToken);
            break;
    }

    // Echo received message text
     
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

//string path = "words.txt";
//Tutor engTutor = new Tutor(path);

////engTutor.AddWord("dog", "собака");
////engTutor.AddWord("cat", "кошка");
////engTutor.AddWord("bird", "птица");
////engTutor.AddWord("fish", "рыба");

//string? word;
//string? translate;

//CheckWordResult result;

//while (true)
//{
//    word = engTutor.GetRandomEngWord();
//    if (word == null)
//    {
//        Console.WriteLine("Словарь пуст");
//        break; 
//    }

//    Console.Write($"Переведите слово {word}: ");
//    translate = Console.ReadLine();
//    if (translate == null)
//    {
//        Console.WriteLine("Слова нет в словаре");
//        break;
//    }
//    result = engTutor.CheckWord(word, translate);

//    switch (result)
//    {
//        case CheckWordResult.Incorrect:
//            Console.WriteLine($"Неверно. Правильный ответ: \"{engTutor.Translate(word)}\"");
//            break;
//        case CheckWordResult.Correct:
//            Console.WriteLine("Верно!");
//            break;
//        case CheckWordResult.Unknown:
//            Console.WriteLine("Неизвестное слово");
//            break;
//    }
//}