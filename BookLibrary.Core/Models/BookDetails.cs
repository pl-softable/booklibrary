namespace BookLibrary.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class BookDetails
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
        public ICollection<BookPersonLoan> Loans { get; set; }
    }
}