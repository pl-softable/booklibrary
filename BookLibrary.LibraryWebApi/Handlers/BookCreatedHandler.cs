namespace BookLibrary.Library.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Events;
    using Core.Events.Library;
    using DataAccess.Repositories.API;

    public class BookCreatedHandler : IEventHandler<BookCreatedEvent>
    {
        private readonly IEventsRepository<BookCreatedEvent> eventsRepository;

        public BookCreatedHandler(IEventsRepository<BookCreatedEvent> eventsRepository)
        {
            this.eventsRepository = eventsRepository;
        }

        public async Task Handle(BookCreatedEvent notification, CancellationToken cancellationToken)
        {
            await this.eventsRepository.AddEvent(notification);
        }
    }
}