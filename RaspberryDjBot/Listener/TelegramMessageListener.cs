using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using RaspberryDjBot.Commands;
using RaspberryDjBot.Common;
using RaspberryDjBot.Providers;
using Vostok.Logging.Abstractions;

namespace RaspberryDjBot.Listener
{
    public class TelegramMessageListener : IObserver<TelegramMessage>
    {
        private readonly ILog log;
        private readonly IProducerConsumerCollection<MediaContent> queue;
        private readonly IEnumerable<IMediaContentProvider> contentProviders;
        private readonly IEnumerable<ICommandHandler> commandHandlers;

        public TelegramMessageListener(ILog log, IProducerConsumerCollection<MediaContent> queue,
            IEnumerable<IMediaContentProvider> contentProviders, IEnumerable<ICommandHandler> commandHandlers)
        {
            this.log = log;
            this.queue = queue;
            this.contentProviders = contentProviders;
            this.commandHandlers = commandHandlers;
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
            Task.Factory.StartNew(() => OnNextAsync(value));
        }

        private async Task OnNextAsync(TelegramMessage message)
        {
            log.Info("Message Received: {0}", message.Text);

            var text = message.Text;

            if (text.StartsWith("/"))
            {
                try
                {
                    foreach (var handler in commandHandlers)
                    {
                        if (handler.CanHandle(text))
                        {
                            handler.Handle(text);
                            log.Info("Handle command {text} from user {user}", text, message.UserName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Error while handling command {command} from user {user}", text, message.UserName);
                }
            }

            foreach (var provider in contentProviders)
            {
                if (provider.TryParseUrl(text, out var url))
                {
                    try
                    {
                        var content = await provider.GetMediaContent(url);

                        if (queue.TryAdd(content))
                            log.Debug("Add media content to queue");
                        else
                            log.Warn("Could not add media content to queue.");
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, "Error while getting media content from '{0}'", url);
                    }
                }
            }

            log.Info("Could not parse command from message {0}", message);
        }
    }
}
