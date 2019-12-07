namespace BookLibrary.Core.Commands.Library
{
    using System;

    public class LendBookCommand : ICommand
    {
        public Guid CreatedBookId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime LentStart { get; set; }
        public DateTime LentEnd { get; set; }
    }
}