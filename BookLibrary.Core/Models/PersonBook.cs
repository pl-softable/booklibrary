namespace BookLibrary.Core.Models
{
    using System.Collections.Generic;

    public class PersonBook
    {
        public Person Person { get; set; }
        public IEnumerable<BookOnLoan> Books { get; set; }
    }
}