using MihaZupan;
using Telegram.Bot;

namespace RaspberryDjBot.Listener
{
    public static class TelegramClientBuilder
    {
        public static ITelegramBotClient BuildClient(BotConfiguration config)
        {
            if (config.UseProxy)
            {
                var proxy = new HttpToSocks5Proxy(config.ProxyHost, config.ProxyPort, config.ProxyLogin,
                    config.ProxyPassword);
                return new TelegramBotClient(config.AccessToken, proxy);
            }

            return new TelegramBotClient(config.AccessToken);
        }
    }
}
