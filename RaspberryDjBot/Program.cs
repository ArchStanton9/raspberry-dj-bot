using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using RaspberryDjBot.Commands;
using RaspberryDjBot.Common;
using RaspberryDjBot.Listener;
using RaspberryDjBot.Player;
using RaspberryDjBot.Providers;
using Vostok.Configuration;
using Vostok.Configuration.Sources.Yaml;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.File;
using Vostok.Logging.File.Configuration;

namespace RaspberryDjBot
{
    public class Program
    {
        // ReSharper disable once ArrangeTypeMemberModifiers
        public static void Main(string[] args)
        {
            var provider = new ConfigurationProvider();
            provider.SetupSourceFor<BotConfiguration>(new YamlFileSource("config.yaml"));
            var config = provider.Get<BotConfiguration>();

            var fileLog = new FileLog(() => new FileLogSettings
            {
                FilePath = "Logs/bot.log",
                Encoding = Encoding.UTF8,
            });

            var consoleLog = new ConsoleLog(new ConsoleLogSettings {ColorsEnabled = true});
            var log = new CompositeLog(fileLog, consoleLog);

            var client = TelegramClientBuilder.BuildClient(config);
            var me = client.GetMeAsync().GetAwaiter().GetResult();
            log.Info(me.ToString());

            var bot = new ReactiveTelegramBot(client);
            var queue = new ConcurrentQueue<MediaContent>();
            var player = new OmxShellMediaPlayer(queue, log);

            var listener = new TelegramMessageListener(log, queue,
                new List<IMediaContentProvider>
                {
                    new YoutubeVideoProvider()
                },
                new List<ICommandHandler>
                {
                    new PlayerCommandHandler(player)
                });

            using (bot.Subscribe(listener))
            {
                client.StartReceiving();
                player.Start();

                Console.ReadLine();
            }
        }
    }
}
