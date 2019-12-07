namespace BookLibrary.Library.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Commands;
    using Core.Commands.Library;
    using Core.Events;
    using Core.Events.Library;
    using MediatR;
    using MongoDB.Bson;

    public class AddBookHandler : ICommandHandler<AddBookCommand>
    {
        private readonly IEventBus eventBus;

        public AddBookHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public async Task<Unit> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var @event = new BookCreatedEvent
            {
                Id = request.Id,
                CreationDate = DateTime.UtcNow,
                Type = nameof(BookCreatedEvent),
                Quantity = request.Quantity,
                Title = request.Title,
                Author = request.Author
            };

            await this.eventBus.Publish(@event);

            return Unit.Value;
        }
    }
}