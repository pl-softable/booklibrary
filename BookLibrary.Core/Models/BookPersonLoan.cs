namespace BookLibrary.Core.Models
{
    using System;

    public class BookPersonLoan
    {
        public Person Person { get; set; }
        public DateTime LoanBeginDate { get; set; }
        public DateTime LoanEndDate { get; set; }
    }
}