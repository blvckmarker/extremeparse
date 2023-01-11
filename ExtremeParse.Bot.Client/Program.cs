using ExtremeParse.Bot.Client.Commands;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

var env = Environment.CurrentDirectory + '/' + Assembly.GetExecutingAssembly().GetName().Name;

#if DEBUG
//currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
#endif

var token = Environment.GetEnvironmentVariable("TOKEN")!;
using var cts = new CancellationTokenSource();


var files = Directory.GetFiles($"./Articles", "*.html");
var articles = files.Select(file => new Article(
    Name: Path.GetFileNameWithoutExtension(file),
    Value: new InputTextMessageContent(System.IO.File.ReadAllText($"./Articles/{file.Split("/")[^1]}"))
    {
        ParseMode = ParseMode.Html
    })).ToList();

var bot = new TelegramBotClient(token);
bot.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: PollingErrorHandler,
    receiverOptions: null,
    cancellationToken: cts.Token);


var me = await bot.GetMeAsync();
Console.WriteLine($"@{me.Username} start receiving");

Console.ReadLine();
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    var chatid = update.Message.Chat.Id;
    try
    {
        await (update.Type switch
        {
            UpdateType.Message => MessageHandlerAsync(botClient, chatid, update.Message),
            _ => ExceptionTypeHandlerAsync(botClient, chatid, update.Type)
        });
    }
    catch (Exception ex)
    {
        await Console.Out.WriteLineAsync(ex.Message);
    }
}

async Task MessageHandlerAsync(ITelegramBotClient botClient, ChatId id, Message message)
{
    var Commands = new List<IBotCommand>()
    {
        new FindByPhone(),
        new FindByName()
    };

    var msg = message.Text.Split(' ');
    var commandName = msg.First();

    if (!new[] { Commands[0].CommandName, Commands[1].CommandName }.Contains(commandName))
    {
        await botClient.SendTextMessageAsync(id,
            $"Ooops!\nSomething went wrong. Maybe I don't know <code>{commandName}</code> command!", //To article
            parseMode: ParseMode.Html);
        return;
    }

    await Commands.
        FirstOrDefault(command => command.CommandName == commandName)!.
        ExecuteAsync(botClient, id, string.Join(' ', msg[1..]));
}

async Task ExceptionTypeHandlerAsync(ITelegramBotClient bot, ChatId chatId, UpdateType? updateType = null) =>
    await bot.SendTextMessageAsync(
        chatId: chatId,
        text: articles.First(article => article.Name == "ExceptionType").Value.MessageText + $"\nTypeError: {updateType}");


Task PollingErrorHandler(ITelegramBotClient bot, Exception ex, CancellationToken ct)
{
    Console.WriteLine($"Exception while polling for updates: {ex.Message}");
    return Task.CompletedTask;
}
record class Article(string Name, InputTextMessageContent Value);