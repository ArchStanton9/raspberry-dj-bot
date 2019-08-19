using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using RaspberryDjBot.Common;
using RaspberryDjBot.Shell;
using Vostok.Logging.Abstractions;

namespace RaspberryDjBot.Player
{
    public class OmxShellMediaPlayer : IMediaPlayer
    {
        private readonly IProducerConsumerCollection<MediaContent> playbackQueue;
        private readonly ILog log;
        private readonly CancellationTokenSource tokenSource;
        private Task playerTask;

        public OmxShellMediaPlayer(IProducerConsumerCollection<MediaContent> playbackQueue, ILog log)
        {
            this.playbackQueue = playbackQueue;
            this.log = log;
            tokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            if (playerTask != null)
                return;

            playerTask = StartInternalAsync(tokenSource.Token);

        }
        
        private async Task StartInternalAsync(CancellationToken token)
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

        public void Play()
        {
            var result = ShellRunner.ExecuteShellCommand("./dbuscontrol.sh", "play", 10000)
                .GetAwaiter()
                .GetResult();

            if (result.ExitCode == 0)
            {
                log.Debug("play");
                return;
            }

            log.Warn("Can not run play command. Code: {0}. Out: {1}", result.ExitCode, result.Output);
        }


        public void Pause()
        {
            var result = ShellRunner.ExecuteShellCommand("./dbuscontrol.sh", "pause", 10000)
                .GetAwaiter()
                .GetResult();

            if (result.ExitCode == 0)
            {
                log.Debug(nameof(Pause));
                return;
            }

            log.Warn("Can not run 'pause' command. Code: {0}. Out: {1}", result.ExitCode, result.Output);
        }

        public void Next()
        {
            var result = ShellRunner.ExecuteShellCommand("./dbuscontrol.sh", ">", 10000)
                .GetAwaiter()
                .GetResult();

            if (result.ExitCode == 0)
            {
                log.Debug(nameof(Next));
                return;
            }

            log.Warn("Can not run 'next' command. Code: {0}. Out: {1}", result.ExitCode, result.Output);
        }
    }
}
