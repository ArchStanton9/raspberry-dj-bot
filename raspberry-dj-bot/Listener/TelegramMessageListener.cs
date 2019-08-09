using System;
using System.Collections.Concurrent;
using raspberry_dj_bot.Commands;
using raspberry_dj_bot.Primitives;
using Vostok.Logging.Abstractions;

namespace raspberry_dj_bot.Listener
{
    public class TelegramMessageListener : IObserver<TelegramMessage>
    {
        private readonly ILog log;
        private readonly IProducerConsumerCollection<IBotCommand> queue;
        private readonly IBotCommandParser parser;

        public TelegramMessageListener(ILog log, IProducerConsumerCollection<IBotCommand> queue,
            IBotCommandParser parser)
        {
            this.log = log;
            this.queue = queue;
            this.parser = parser;
        }

        public void OnCompleted()
        {
            log.Info("Stop listening for messages. Queue: {0}", queue.ToArray().Length);
        }

        public void OnError(Exception error)
        {
            log.Error(error);
        }

        public void OnNext(TelegramMessage value)
        {
            log.Info("Message Received: {0}", value.Args.Message.Text);
            if (parser.TryParseCommand(value, out var command))
            {
                if (queue.TryAdd(command))
                    log.Debug("Add command to queue");
                else
                    log.Warn("Could not add command to queue.");

                return;
            }

            log.Info("Could not parse command from message {0}", value.ToString());
        }
    }
}
