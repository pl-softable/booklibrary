namespace BookLibrary.Core.Queries.Identity
{
    using System;
    using Models;

    public class GetPersonQuery : IQuery<Person>
    {
        public Guid Id { get; set; }
    }
}