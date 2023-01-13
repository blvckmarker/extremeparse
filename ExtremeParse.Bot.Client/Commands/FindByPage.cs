﻿using Newtonsoft.Json;
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

        public async Task<Message> ExecuteAsync(ITelegramBotClient botClient, ChatId chatid, Message message, CancellationToken? ctsToken = null)
        {
            var args = string.Join(" ", message.Text.Split(' ')[1..]);
            if (!Regex.IsMatch(args, "https://m?.?vk.com/id\\d+}") && !Regex.IsMatch(args, "https://m?.?vk.com/\\w+"))
                return await botClient.SendTextMessageAsync(chatid,
                    "Invalid user account link. Use this template:\n `<code>https://vk.com/id{user id}</code>` or \n`<code>https://vk.com/{short name}</code>`",
                    parseMode: ParseMode.Html);

            var data = new
            {
                Name = "Samuel",
                Description = "hallo world",
                Info = "ssds",
                Photo = "https://",
                Id = Guid.NewGuid(),
                DataTime = DateTime.Now
            };
            //await ParserMediator.DispatchData(CommandName, args);

            return await botClient.SendTextMessageAsync(
                chatId: chatid,
                replyToMessageId: message.MessageId,
                text: "<b>[Do you want to publish this information?]</b>\n" + $"<code>{JsonConvert.SerializeObject(data, Formatting.Indented)}</code>",
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
