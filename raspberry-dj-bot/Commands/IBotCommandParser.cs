using raspberry_dj_bot.Primitives;

namespace raspberry_dj_bot.Commands
{
    public interface IBotCommandParser
    {
        bool TryParseCommand(TelegramMessage message, out IBotCommand command);
    }
}
