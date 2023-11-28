using Telegram;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

TelegramBotClient botClient = new TelegramBotClient("6894971870:AAHpiIXFM5bCX8dSk0VOqh-BgnNjt2hbAgU");

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

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    // Echo received message text
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You said:\n" + messageText,
        cancellationToken: cancellationToken);
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