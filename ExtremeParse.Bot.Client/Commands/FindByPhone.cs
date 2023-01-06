using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExtremeParse.Bot.Client.Commands
{
    internal class FindByPhone : IBotCommand
    {
        public string CommandName => "/findbyphone";
        public string CommandDescription => "findbyphone - Find information about human using him/her phone number";

        public async Task ExecuteAsync(ITelegramBotClient botClient, ChatId chatid, string args)
        {
            await botClient.SendTextMessageAsync(chatid, $"Че сука а {args}");
        }
    }
}
