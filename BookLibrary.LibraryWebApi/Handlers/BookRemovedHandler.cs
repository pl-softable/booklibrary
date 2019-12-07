namespace BookLibrary.Library.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Events;
    using Core.Events.Library;
    using DataAccess.Repositories.API;

    public class BookRemovedHandler : IEventHandler<BookRemovedEvent>
    {
        private readonly IEventsRepository<BookRemovedEvent> eventsRepository;

        public BookRemovedHandler(IEventsRepository<BookRemovedEvent> eventsRepository)
        {
            this.eventsRepository = eventsRepository;
        }

        public async Task Handle(BookRemovedEvent notification, CancellationToken cancellationToken)
        {
            await this.eventsRepository.AddEvent(notification);
        }
    }
}