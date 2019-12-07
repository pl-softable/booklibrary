namespace BookLibrary.Core.Events
{
    using System.Threading.Tasks;
    using MediatR;

    public class EventBus : IEventBus
    {
        private readonly IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Publish<TEvent>(params TEvent[] events) where TEvent : IEvent
        {
            foreach (var @event in events) await this._mediator.Publish(@event);
        }
    }
}