namespace BookLibrary.Library.Handlers
{
    using System;
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

    public class GetPersonDetailsHandler : IQueryHandler<GetPersonDetailsQuery, PersonBook>
    {
        private readonly IEventsRepository<BookLentEvent> bookLentsEventsRepository;
        private readonly IConfiguration configuration;

        public GetPersonDetailsHandler(IConfiguration configuration,
            IEventsRepository<BookLentEvent> bookLentsEventsRepository)
        {
            this.configuration = configuration;
            this.bookLentsEventsRepository = bookLentsEventsRepository;
        }

        public async Task<PersonBook> Handle(GetPersonDetailsQuery request, CancellationToken cancellationToken)
        {
            var books = await this.bookLentsEventsRepository.GetAllEvents();

            var person = await HttpHelper.GetApiResponse<Person>(this.configuration["ApiGatewayUrl"],
                $"/api/getPerson/{request.PersonId}");

            var lentBooks = books.Where(item => item.PersonId == request.PersonId)
                .Where(item => item.LentEnd >= DateTime.UtcNow);

            var personBook = new PersonBook
            {
                Person = person,
                Books = lentBooks.Select(item => new BookOnLoan
                {
                    BookId = item.BookId,
                    LentStart = item.LentStart,
                    LentEnd = item.LentEnd
                })
            };

            return personBook;
        }
    }
}