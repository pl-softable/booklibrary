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
    using Microsoft.Extensions.Configuration;

    public class LendBookHandler : ICommandHandler<LendBookCommand>
    {
        private readonly IConfiguration configuration;
        private readonly IEventsRepository<BookCreatedEvent> createdBooksRepository;
        private readonly IEventBus eventBus;
        private readonly IEventsRepository<BookLentEvent> lentBooksRepository;
        private readonly IEventsRepository<BookRemovedEvent> removedBooksRepository;

        public LendBookHandler(IEventBus eventBus,
            IConfiguration configuration,
            IEventsRepository<BookCreatedEvent> createdBooksRepository,
            IEventsRepository<BookRemovedEvent> removedBooksRepository,
            IEventsRepository<BookLentEvent> lentBooksRepository)
        {
            this.eventBus = eventBus;
            this.createdBooksRepository = createdBooksRepository;
            this.removedBooksRepository = removedBooksRepository;
            this.lentBooksRepository = lentBooksRepository;
            this.configuration = configuration;
        }

        public async Task<Unit> Handle(LendBookCommand request, CancellationToken cancellationToken)
        {
            var createdBook = await this.createdBooksRepository
                .GetEvent(request.CreatedBookId);

            var removedBooks = await this.removedBooksRepository
                .GetAllEventsForBook(request.CreatedBookId);

            var entireLentBooks = await this.lentBooksRepository
                .GetAllEventsForBook(request.CreatedBookId);

            var removedBooksQuantity = removedBooks.Sum(item => item.Quantity);

            if (createdBook.Quantity - removedBooksQuantity <= 0) throw new Exception("BookOnLoan is not available.");

            var lentBooks = entireLentBooks.Where(item => item.LentEnd > item.LentStart);

            var booksLentForPerson = entireLentBooks
                .Where(item => item.PersonId == request.PersonId)
                .Where(item => item.LentEnd <= DateTime.UtcNow);

            if (lentBooks.Count() >= createdBook.Quantity - removedBooksQuantity)
                throw new Exception("All books were lent.");

            if (booksLentForPerson.Count() > 4) throw new Exception("Person exceeds the limit of lent books.");

            var @event = new BookLentEvent
            {
                BookId = request.CreatedBookId,
                PersonId = request.PersonId,
                CreationDate = DateTime.UtcNow,
                LentStart = request.LentStart,
                LentEnd = request.LentEnd,
                Type = nameof(BookLentEvent)
            };

            await this.eventBus.Publish(@event);

            return Unit.Value;
        }
    }
}