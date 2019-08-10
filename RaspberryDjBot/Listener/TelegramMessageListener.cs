using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using RaspberryDjBot.Common;
using RaspberryDjBot.YouTube;
using Vostok.Logging.Abstractions;

namespace RaspberryDjBot.Listener
{
    public class TelegramMessageListener : IObserver<TelegramMessage>
    {
        private readonly ILog log;
        private readonly IProducerConsumerCollection<MediaContent> queue;
        

        public TelegramMessageListener(ILog log, IProducerConsumerCollection<MediaContent> queue)
        {
            this.log = log;
            this.queue = queue;
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
            var urlType = UrlTypeParser.TryParser(message.Text, out var url);

            if (urlType == UrlType.None)
            {
                log.Info("Could not parse command from message {0}", message);
                return;
            }

            var content = default(MediaContent);

            if (urlType == UrlType.Youtube)
            {
                var youtubeProvider = new YoutubeVideoProvider();
                content = await youtubeProvider.GetYoutubeVideo(url);
            }
            
            if (queue.TryAdd(content))
                log.Debug("Add media content to queue");
            else
                log.Warn("Could not add media content to queue.");
        }
    }
}
