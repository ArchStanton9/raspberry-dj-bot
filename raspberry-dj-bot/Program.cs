using System;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Vostok.Configuration;
using Vostok.Configuration.Sources.Yaml;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.File;
using Vostok.Logging.File.Configuration;

namespace raspberry_dj_bot
{
    public class Program
    {
        // ReSharper disable once ArrangeTypeMemberModifiers
        public static async Task Main(string[] args)
        {
            var provider = new ConfigurationProvider();
            provider.SetupSourceFor<BotConfiguration>(new YamlFileSource("config.yaml"));
            var config = provider.Get<BotConfiguration>();

            var client = new TelegramBotClient(config.AccessToken);
            
            var fileLog = new FileLog(() => new FileLogSettings
            {
                FilePath = "Logs/bot.log",
                Encoding = Encoding.UTF8,
            });

            var consoleLog = new ConsoleLog(new ConsoleLogSettings {ColorsEnabled = true});
            var log = new CompositeLog(fileLog, consoleLog);
            
            var bot = new ReactiveTelegramBot(client);
            var listener = new TelegramMessageListener(log);
            using (bot.Subscribe(listener))
            {
                client.StartReceiving();
                Console.ReadLine();
            }
        }
    }
}
