namespace RaspberryDjBot.Commands
{
    public interface ICommandHandler
    {
        bool CanHandle(string command);

        void Handle(string command);
    }
}
