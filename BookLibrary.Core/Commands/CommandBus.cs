namespace BookLibrary.Core.Commands
{
    using System.Threading.Tasks;
    using MediatR;

    public class CommandBus : ICommandBus
    {
        private readonly IMediator _mediator;

        public CommandBus(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            return this._mediator.Send(command);
        }
    }
}