using System;
using System.Linq;
using System.Threading.Tasks;
using RaspberryDjBot.Common;
using RaspberryDjBot.Shell;

namespace RaspberryDjBot.YouTube
{
    public class YoutubeVideoProvider
    {
        public async Task<MediaContent> GetYoutubeVideo(Uri url)
        {
            var output = await ShellRunner.RunCommandAsync("youtube-dl",
                "-e",
                "--get-duration",
                "-g",
                url.ToString()
            );

            var lines = output.Split("\n");
            var info = new MediaContent();
            info.Title = lines[0];
            info.Source = new Uri(lines[1]);
            info.Duration = lines.Last();

            return info;
        }
    }
}
