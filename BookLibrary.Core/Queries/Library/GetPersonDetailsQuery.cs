namespace BookLibrary.Core.Queries.Library
{
    using System;
    using Models;

    public class GetPersonDetailsQuery : IQuery<PersonBook>
    {
        public Guid PersonId { get; set; }
    }
}