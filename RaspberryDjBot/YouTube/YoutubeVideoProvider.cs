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
            var result = await ShellRunner.ExecuteShellCommand(
                "youtube-dl",
                $"-e --get-duration -g {source}", 30000);

            if (result.ExitCode != 0)
                throw new Exception("Failed to load content");

            var lines = result.Output.Split("\n");
            var info = new MediaContent();
            info.Title = lines[0];
            info.Source = new Uri(lines[1]);
            info.Duration = lines.Last();

            return info;
        }
    }
}
