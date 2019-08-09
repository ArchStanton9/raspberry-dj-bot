using System;
using raspberry_dj_bot.Primitives;
using Telegram.Bot;

namespace raspberry_dj_bot.Listener
{
    public class ReactiveTelegramBot : IObservable<TelegramMessage>
    {
        private readonly ITelegramBotClient client;

        public ReactiveTelegramBot(ITelegramBotClient client)
        {
            this.client = client;
        }

        public IDisposable Subscribe(IObserver<TelegramMessage> observer)
        {
            return new BotSubscription(client, observer);
        }
    }
}
