using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using raspberry_dj_bot.Shell;

namespace raspberry_dj_bot.YouTube
{
    public class YoutubeMetaInfoProvider
    {
        public YoutubeMetaInfoProvider()
        {

        }

        public YoutubeVideoInfo GetVideoInfo(Uri url)
        {
            var output = ShellCommandRunner.RunCommand("youtube-dl",
                "-e",
                "--get-duration",
                "-g",
                url.ToString()
            );

            var lines = output.Split("\n");
            var info = new YoutubeVideoInfo();
            info.Title = lines[0];
            info.Source = new Uri(lines[1]);
            info.Duration = lines.Last();

            return info;
        }
    }
}
