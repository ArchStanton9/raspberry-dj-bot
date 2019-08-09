using System;
using raspberry_dj_bot.Primitives;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace raspberry_dj_bot.Listener
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
                Args = e,
                Text = e.Message.Text,
                ChatId = e.Message.Chat.Id
            });
        }

        public void Dispose()
        {
            observer.OnCompleted();
            client.OnMessage -= ClientOnOnMessage;
        }
    }
}