using System;
using RaspberryDjBot.Common;
using Telegram.Bot;

namespace RaspberryDjBot.Listener
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
