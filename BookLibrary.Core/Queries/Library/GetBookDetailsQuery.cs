namespace BookLibrary.Core.Queries.Library
{
    using System;
    using Models;
    using MongoDB.Bson;

    public class GetBookDetailsQuery : IQuery<BookDetails>
    {
        public GetBookDetailsQuery(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; set; }
    }
}