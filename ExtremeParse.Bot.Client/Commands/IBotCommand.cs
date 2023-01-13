using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExtremeParse.Bot.Client.Commands
{
    internal interface IBotCommand
    {
        public string CommandName { get; }
        public string CommandDescription { get; }

        public Task<Message> ExecuteAsync(ITelegramBotClient botClient, ChatId chatid, Message message, CancellationToken? ctsToken = null);
    }
}
