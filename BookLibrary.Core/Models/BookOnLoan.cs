using System;
using System.Collections.Generic;
using System.Text;

namespace BookLibrary.Core.Models
{
    public class BookOnLoan
    {
        public Guid BookId { get; set; }
        public DateTime LentStart { get; set; }
        public DateTime LentEnd { get; set; }
    }
}
