using BookLibrary.Core.Commands.Identity;

namespace BookLibrary.Identity.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Commands;
    using DataAccess.Contexts;
    using DataAccess.Models;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class AddPersonHandler : ICommandHandler<AddPersonCommand>
    {
        private readonly ApplicationDbContext dbContext;

        public AddPersonHandler(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            var isPersonExistsWithTheSameSSN = await this.dbContext.RegisteredPersons
                .AnyAsync(item => item.SSN == request.SSN);

            if (isPersonExistsWithTheSameSSN)
            {
                throw new Exception("Person with the same SSN exists.");
            }

            await this.dbContext.RegisteredPersons.AddAsync(new RegisteredPerson()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                SSN = request.SSN
            });

            await this.dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}