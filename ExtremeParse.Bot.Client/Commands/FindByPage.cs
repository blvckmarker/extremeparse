using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExtremeParse.Bot.Client.Commands
{
    internal class FindByPage : IBotCommand
    {
        public string CommandName => "/findbypage";
        public string CommandDescription => "Find information about human using him/her username";

        public async Task<Message> ExecuteAsync(ITelegramBotClient botClient, ChatId chatid, string args, CancellationToken? ctsToken = null)
        {
            if (!Regex.IsMatch(args, "https://m?.?vk.com/id\\d+}") && !Regex.IsMatch(args, "https://m?.?vk.com/\\w+"))
                return await botClient.SendTextMessageAsync(chatid,
                    "Invalid user account link. Use this template:\n `<code>https://vk.com/id{user id}</code>` or \n`<code>https://vk.com/{short name}</code>`",
                    parseMode: ParseMode.Html);

            //var data = await ParserMediator.DispatchData(CommandName, args);

            var msg = await botClient.SendTextMessageAsync(
            chatId: chatid,
            text: "Trying *all the parameters* of `sendMessage` method",
            parseMode: ParseMode.MarkdownV2,
            disableNotification: true,
            replyMarkup: new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Yes, publish!", "publish"),
                InlineKeyboardButton.WithCallbackData("No, stop", "none"),
            }));

            var a = msg.ReplyMarkup.InlineKeyboard.ElementAt(0).ElementAt(0).CallbackData;

            return new Message();
            //return await botClient.SendTextMessageAsync(chatid,
            //    "Do you want to publish this data?\n" +
            //    "|-> <b>Name</b>: {data.Name}\n" +
            //    "|-> <b>Description</b>: {data.Description}\n" +
            //    "|-> <b>Info</b>: {data.Info}\n" +
            //    "|-> <b>Number</b>: {data.Number}",
            //    replyMarkup: new InlineKeyboardMarkup(
            //        new InlineKeyboardButton(text: "Yes, publish!")),
            //    parseMode: ParseMode.Html);
        }
    }
}
