namespace raspberry_dj_bot.Handler
{
    public interface ICommandHandler<in TCommand>
    {
        void HandleCommand(TCommand command);
    }
}
