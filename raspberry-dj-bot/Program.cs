using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Vostok.Configuration;
using Vostok.Configuration.Sources.Yaml;

namespace raspberry_dj_bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = new ConfigurationProvider();
            provider.SetupSourceFor<BotConfiguration>(new YamlFileSource("config.yaml"));
            var config = provider.Get<BotConfiguration>();

            var client = new TelegramBotClient(config.AccessToken);
            var me = await client.GetMeAsync();

            Console.Write($"Hello! I'm {me.Username}!");
        }
    }
}
