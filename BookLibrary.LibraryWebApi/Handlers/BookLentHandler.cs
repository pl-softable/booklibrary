namespace BookLibrary.Library.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Events;
    using Core.Events.Library;
    using DataAccess.Repositories.API;

    public class BookLentHandler : IEventHandler<BookLentEvent>
    {
        private readonly IEventsRepository<BookLentEvent> eventsRepository;

        public BookLentHandler(IEventsRepository<BookLentEvent> eventsRepository)
        {
            this.eventsRepository = eventsRepository;
        }

        public async Task Handle(BookLentEvent notification, CancellationToken cancellationToken)
        {
            await this.eventsRepository.AddEvent(notification);
        }
    }
}