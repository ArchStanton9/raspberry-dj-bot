using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace raspberry_dj_bot
{
    public class BotSubscription : IDisposable
    {
        private readonly ITelegramBotClient client;
        private readonly IObserver<TelegramMessage> observer;

        public BotSubscription(ITelegramBotClient client, IObserver<TelegramMessage> observer)
        {
            this.client = client;
            this.observer = observer;
            client.OnMessage += ClientOnOnMessage;
            client.OnReceiveError += ClientOnOnReceiveError;
        }

        private void ClientOnOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            observer.OnError(e.ApiRequestException);
        }

        private void ClientOnOnMessage(object sender, MessageEventArgs e)
        {
            observer.OnNext(new TelegramMessage
            {
                Args = e
            });
        }

        public void Dispose()
        {
            observer.OnCompleted();
            client.OnMessage -= ClientOnOnMessage;
        }
    }
}