namespace BookLibrary.Library.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Events.Library;
    using Core.Models;
    using Core.Queries;
    using Core.Queries.Library;
    using DataAccess.Repositories.API;
    using Microsoft.Extensions.Configuration;

    public class GetBooksDetailsHandler : IQueryHandler<GetBookDetailsQuery, BookDetails>
    {
        private readonly IEventsRepository<BookCreatedEvent> createdBooksRepository;
        private readonly IEventsRepository<BookLentEvent> lentBooksRepository;
        private readonly IEventsRepository<BookRemovedEvent> removedBooksRepository;
        private readonly IConfiguration configuration;

        public GetBooksDetailsHandler(IEventsRepository<BookCreatedEvent> createdBooksRepository,
            IEventsRepository<BookRemovedEvent> removedBooksRepository,
            IEventsRepository<BookLentEvent> lentBooksRepository,
            IConfiguration configuration)
        {
            this.createdBooksRepository = createdBooksRepository;
            this.removedBooksRepository = removedBooksRepository;
            this.lentBooksRepository = lentBooksRepository;
            this.configuration = configuration;
        }

        public async Task<BookDetails> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            var createdResult = await this.createdBooksRepository.GetEvent(request.BookId);

            var removedResult = await this.removedBooksRepository.GetAllEventsForBook(request.BookId);

            var quantities = createdResult.Quantity - removedResult.Sum(item => item.Quantity);

            var loans = await this.lentBooksRepository.GetAllEventsForBook(request.BookId);

            var currentLoans = loans.Where(item => item.LentEnd >= DateTime.UtcNow);

            var bookDetails = new BookDetails
            {
                BookId = createdResult.Id,
                Author = createdResult.Author,
                Title = createdResult.Title,
                Quantity = quantities,
                Loans = new List<BookPersonLoan>()
            };

            foreach (var bookLentEvent in currentLoans)
            {
                bookDetails.Loans.Add(await this.CreateBookPersonLoan(bookLentEvent));
            }

            return bookDetails;
        }

        private async Task<BookPersonLoan> CreateBookPersonLoan(BookLentEvent @event)
        {
            var result = new BookPersonLoan()
            {
                LoanBeginDate = @event.LentStart,
                LoanEndDate = @event.LentEnd,
                Person = await HttpHelper.GetApiResponse<Person>(
                    this.configuration["ApiGatewayUrl"], $"/api/getPerson/{@event.PersonId}")
            };

            return result;
        }
    }
}