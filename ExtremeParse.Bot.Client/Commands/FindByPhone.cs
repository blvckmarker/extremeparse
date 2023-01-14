using ExtremeParse.Bot.Client.Mediator.Parser;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExtremeParse.Bot.Client.Commands
{
    internal class FindByPhone : IBotCommand
    {
        public string CommandName => "/findbyphone";
        public string CommandDescription => "findbyphone - e.g +78005553535 | Find information about human using him/her phone number [Only +7]";

        public async Task<Message> ExecuteAsync(ITelegramBotClient botClient, ChatId chatid, Message message, CancellationToken? ctsToken = null)
        {
            var args = message.Text.Split(' ').ElementAtOrDefault(1) ?? string.Empty;
            if (!Regex.IsMatch(args, "\\+7\\d{10}") && !Regex.IsMatch(args, "8\\d{10}"))
                return await botClient.SendTextMessageAsync(chatid,
                    "❌ | Incorrect input number format. Please, keep template such as this: <code>+78005553535</code>",
                    ParseMode.Html);

            var data = await ParserMediator.DispatchData(CommandName, args);

            return await botClient.SendTextMessageAsync(
                chatId: chatid,
                replyToMessageId: message.MessageId,
                text: "❗ <b>[Do you want to publish this information?]</b>\n" + $"<code>{JsonConvert.SerializeObject(data, Formatting.Indented)}</code>",
                disableNotification: true,
                parseMode: ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithCallbackData("Yes, publish!", "publish"),
                    InlineKeyboardButton.WithCallbackData("No, stop", "none"),
                }));
        }
    }
}
