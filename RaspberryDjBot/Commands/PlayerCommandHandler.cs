using System.Collections.Generic;
using RaspberryDjBot.Player;

namespace RaspberryDjBot.Commands
{
    public class PlayerCommandHandler : ICommandHandler
    {
        private readonly IMediaPlayer player;

        public PlayerCommandHandler(IMediaPlayer player)
        {
            this.player = player;
        }

        private static readonly HashSet<string> playerCommands = new HashSet<string>()
        {
            "/play",
            "/pause",
            "/next"
        };

        public bool CanHandle(string command) => playerCommands.Contains(command);

        public void Handle(string command)
        {
            switch (command)
            {
                case "/play":
                    player.Play();
                    break;
                case "/pause":
                    player.Pause();
                    break;
                case "/next":
                    player.Play();
                    break;
            }
        }
    }
}
