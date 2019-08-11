using System;
using System.IO;
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
            var source = url.ToString();
            var output = await ShellRunner.RunCommandAsync("youtube-dl",
                "-e",
                "--get-duration",
                "-g",
                "https://www.youtube.com/watch?v=guaEK62mxno" //source
            ).ConfigureAwait(false);

            var lines = output.Split("\n");
            var info = new MediaContent();
            info.Title = lines[0];
            info.Source = new Uri(lines[1]);
            info.Duration = lines.Last();

            return info;
        }
    }
}
