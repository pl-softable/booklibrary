namespace BookLibrary.Identity.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Models;
    using Core.Queries;
    using Core.Queries.Identity;
    using DataAccess.Contexts;
    using DataAccess.Models;
    using Microsoft.EntityFrameworkCore;

    public class GetPersonHandler : IQueryHandler<GetPersonQuery, Person>
    {
        private readonly ApplicationDbContext dbContext;

        public GetPersonHandler(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Person> Handle(GetPersonQuery request, CancellationToken cancellationToken)
        {
            var person =
                await this.dbContext.RegisteredPersons.FirstOrDefaultAsync(item => item.PersonId == request.Id);

            if (person == null)
            {
                throw new Exception("Person does not exists.");
            }

            var registeredPerson = new Person(person.PersonId, 
                person.FirstName, person.LastName, person.SSN);

            return registeredPerson;
        }
    }
}