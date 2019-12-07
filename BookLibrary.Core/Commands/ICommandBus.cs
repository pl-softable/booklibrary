namespace BookLibrary.Core.Commands
{
    using System.Threading.Tasks;

    public interface ICommandBus
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}