using ExtremeParse.Bot.Client.Commands;
using ExtremeParse.Bot.Client.Mediator.Site.Server;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using ExtremeParse.Models;

var env = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;
//Environment.CurrentDirectory + '/' + Assembly.GetExecutingAssembly().GetName().Name;

#if DEBUG
//currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
#endif

var token = Environment.GetEnvironmentVariable("TOKEN")!;
using var cts = new CancellationTokenSource();




var files = Directory.GetFiles($"{env}\\Articles", "*.html");
var articles = files.Select(file => new Article(
    Name: Path.GetFileNameWithoutExtension(file),
    Value: new InputTextMessageContent(System.IO.File.ReadAllText($"{env}/Articles/{file.Split("\\")[^1]}"))
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
    try
    {
        await (update.Type switch
        {
            UpdateType.Message => MessageHandlerAsync(botClient, update.Message.Chat.Id, update.Message, token),
            UpdateType.CallbackQuery => CallbackQueryHandlerAsync(botClient, update.CallbackQuery),
            _ => ExceptionTypeHandlerAsync(botClient, update.Message.Chat.Id, update.Type)
        });
    }
    catch (Exception ex)
    {
        await Console.Out.WriteLineAsync(ex.Message);
    }
}

async Task MessageHandlerAsync(ITelegramBotClient botClient, ChatId id, Message message, CancellationToken? cts = null)
{
    var Commands = new List<IBotCommand>()
    {
        new FindByPhone(),
        new FindByPage()
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
        ExecuteAsync(botClient, id, message, cts);
}

async Task CallbackQueryHandlerAsync(ITelegramBotClient botClient, CallbackQuery callback)
{
    var chatId = callback.Message.Chat.Id;
    var context = callback.Data switch
    {
        "publish" => new { ToPublish = true, Text = "OK✅. Sending request" },
        "none" => new { ToPublish = false, Text = "OK✅. Nothing" },
        _ => new { ToPublish = false, Text = "Someting went wrong. Try again!" }
    };
    if (!context.ToPublish)
    {
        await botClient.DeleteMessageAsync(chatId, callback.Message.MessageId);
        await SendMsg(context.Text, chatId);
        return;
    }
    await SendMsg(context.Text, chatId);

    var previousMsg = Regex.Match(callback.Message.Text.Replace("\n", ""), "\\{(.*?)\\}");
    var data = JsonConvert.DeserializeObject<CardModel>(previousMsg.Value);

    data.Creator = "Telegram-Bot";
    data.DateTime = DateTime.Now;

    await TcpClientSender.SendData(data);

}

async Task ExceptionTypeHandlerAsync(ITelegramBotClient bot, ChatId chatId, UpdateType? updateType = null) =>
    await SendMsg(articles.First(article => article.Name == "ExceptionType").Value.MessageText + $"\nTypeError: {updateType}", chatId);


Task PollingErrorHandler(ITelegramBotClient bot, Exception ex, CancellationToken ct)
{
    Console.WriteLine($"Exception while polling for updates: {ex.Message}");
    return Task.CompletedTask;
}

async Task SendMsg(string message, ChatId id, ParseMode? mode = null)
    => await bot.SendTextMessageAsync(
        text: message,
        chatId: id,
        parseMode: mode);



record class Article(string Name, InputTextMessageContent Value);