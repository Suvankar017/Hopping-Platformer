namespace HoppingPlatformer.Application.Commands
{
    public interface ICommandBus
    {
        void Execute<T>(T command) where T : ICommand;
    }
}