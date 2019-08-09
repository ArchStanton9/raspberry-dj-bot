using System;
using Vostok.Logging.Abstractions;

namespace raspberry_dj_bot
{
    public class TelegramMessageListener : IObserver<TelegramMessage>
    {
        private readonly ILog log;

        public TelegramMessageListener(ILog log)
        {
            this.log = log;
        }

        public void OnCompleted()
        {
            log.Info("Stop listening for messages");
        }

        public void OnError(Exception error)
        {
            log.Error(error);
        }

        public void OnNext(TelegramMessage value)
        {
            log.Info("Message Received: {0}", value.Args.Message.Text);
        }
    }
}
