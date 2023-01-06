using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExtremeParse.Bot.Client.Commands
{
    internal class FindByName : IBotCommand
    {
        public string CommandName => "/findbyname";
        public string CommandDescription => "Find information about human using him/her username";

        public Task ExecuteAsync(ITelegramBotClient botClient, ChatId chatid, string args)
        {
            throw new NotImplementedException();
        }
    }
}
