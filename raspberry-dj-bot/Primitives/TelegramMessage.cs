using Telegram.Bot.Args;

namespace raspberry_dj_bot.Primitives
{
    public class TelegramMessage
    {
        public MessageEventArgs Args { get; set; }

        public string Text { get; set; }

        public long ChatId { get; set; }
    }
}
