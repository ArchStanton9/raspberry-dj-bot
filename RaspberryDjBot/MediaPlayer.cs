using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using RaspberryDjBot.Common;
using RaspberryDjBot.Shell;

namespace RaspberryDjBot
{
    public class MediaPlayer
    {
        private readonly IProducerConsumerCollection<MediaContent> playbackQueue;
        private readonly CancellationTokenSource tokenSource;
        private Task playerTask;

        public MediaPlayer(IProducerConsumerCollection<MediaContent> playbackQueue)
        {
            this.playbackQueue = playbackQueue;
            tokenSource = new CancellationTokenSource();
        }

        public void Play()
        {
            if (playerTask != null)
                return;

            playerTask = PlayInternalAsync(tokenSource.Token);
        }

        private async Task PlayInternalAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (playbackQueue.TryTake(out var content))
                {
                    await ShellRunner.RunCommandAsync("omxplayer", content.Source.ToString());
                }
                else
                {
                    await Task.Delay(2000, token);
                }
            }
        }

        public void Pause()
        {

        }

        public void Next()
        {

        }
    }
}
