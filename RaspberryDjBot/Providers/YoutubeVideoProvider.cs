using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RaspberryDjBot.Common;
using RaspberryDjBot.Shell;

namespace RaspberryDjBot.Providers
{
    public class YoutubeVideoProvider : IMediaContentProvider
    {
        private static readonly Regex youtubeUrlRegex = new Regex(@".+(youtube\.com)|(youtu\.be).+",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public bool TryParseUrl(string text, out Uri url)
        {
            var match = youtubeUrlRegex.Match(text);
            if (match.Success && Uri.TryCreate(match.Value, UriKind.Absolute, out url))
                return true;

            url = default(Uri);
            return false;
        }

        public async Task<MediaContent> GetMediaContent(Uri url)
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
