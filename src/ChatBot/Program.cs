using Microsoft.Extensions.Configuration;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();
string TOKEN = config["token"] ?? "Токен не задан";

const string COMMAND_LIST = @"Список команд:
/add <eng> <rus> - добавление английского слова и его перевод в словарь
/get - получаем случайное английское слово из словаря
/check <eng> <rus> - проверяем правильность перевода английского слова
/stop - остановить вывод английских слов
";

Dictionary<long, string> userWords = new Dictionary<long, string>();

string path = Environment.GetEnvironmentVariable("Storage") ?? "Хранилище не задано";
Tutor engTutor = new Tutor(path);

TelegramBotClient botClient = new TelegramBotClient(TOKEN);


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
    string text = COMMAND_LIST;
    switch (msgArgs[0])
    {
        case "/stop":
            text = "Игра окончена!";
            userWords.Remove(chatId);
            break;
        case "/help":
        case "/start":
            text = COMMAND_LIST;
            break;
        case "/add":
            text = engTutor.AddWord(msgArgs);
            break;
        case "/get":
            text = GetRandomWord(chatId, engTutor);
            break;
        case "/check":
            text = engTutor.CheckWord(msgArgs);
            text = $"{text}\r\n{GetRandomWord(chatId, engTutor)}";
            break;
        default:
            if (!msgArgs[0].StartsWith('/') && userWords.ContainsKey(chatId))
            {
                text = engTutor.CheckWord(new string[] { "/check", userWords[chatId], msgArgs[0] });
                userWords.Remove(chatId);
                text = $"{text}\r\n{GetRandomWord(chatId, engTutor)}";
            }
            break;
    }

    sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: text,
        cancellationToken: cancellationToken);
    // Echo received message text

}

string GetRandomWord(long chatId, Tutor engTutor)
{
    string text;
    var randomWord = (engTutor.GetRandomEngWord() ?? "Словарь пуст");
    text = randomWord switch
    {
        "Словарь пуст" => "Словарь пуст",
        _ => $"Переведите слово \"{randomWord}\""
    };
    if (text != "Словарь пуст")
        userWords.Add(chatId, randomWord);
    return text;
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