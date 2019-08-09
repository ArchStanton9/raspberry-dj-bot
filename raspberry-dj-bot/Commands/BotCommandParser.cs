using System;
using raspberry_dj_bot.Primitives;

namespace raspberry_dj_bot.Commands
{
    public class BotCommandParser : IBotCommandParser
    {
        public bool TryParseCommand(TelegramMessage message, out IBotCommand command)
        {
            command = null;
            if (message.Text.StartsWith("https://www.youtube.com"))
            {
                if (Uri.TryCreate(message.Text, UriKind.Absolute, out var uri))
                {
                    command = new PlayYoutubeCommand()
                    {
                        VideoUrl = uri,
                        Name = "youtube"
                    };

                    return true;
                }
            }

            return false;
        }
    }
}