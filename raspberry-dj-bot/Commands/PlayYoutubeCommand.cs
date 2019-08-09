using System;

namespace raspberry_dj_bot.Commands
{
    public class PlayYoutubeCommand : IBotCommand
    {
        public Uri VideoUrl { get; set; }

        public string Name { get; set; }
    }
}
