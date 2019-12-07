namespace BookLibrary.Library.Handlers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Commands;
    using Core.Commands.Library;
    using Core.Events;
    using Core.Events.Library;
    using DataAccess.Repositories.API;
    using MediatR;

    public class RemoveBookHandler : ICommandHandler<RemoveBookCommand>
    {
        private readonly IEventsRepository<BookCreatedEvent> createdBooksRepository;
        private readonly IEventBus eventBus;
        private readonly IEventsRepository<BookRemovedEvent> removedBooksRepository;

        public RemoveBookHandler(IEventBus eventBus, IEventsRepository<BookCreatedEvent> createdBooksRepository,
            IEventsRepository<BookRemovedEvent> removedBooksRepository)
        {
            this.eventBus = eventBus;
            this.createdBooksRepository = createdBooksRepository;
            this.removedBooksRepository = removedBooksRepository;
        }

        public async Task<Unit> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            var createdBook = await this.createdBooksRepository.GetEvent(request.BookId);
            var removedBooks = await this.removedBooksRepository.GetAllEventsForBook(request.BookId);

            if (createdBook.Quantity - removedBooks.Sum(item => item.Quantity) - request.Quantity < 0)
                throw new Exception("No books to remove.");

            var @event = new BookRemovedEvent
            {
                BookId = request.BookId,
                CreationDate = DateTime.UtcNow,
                Quantity = request.Quantity
            };

            await this.eventBus.Publish(@event);

            return Unit.Value;
        }
    }
}