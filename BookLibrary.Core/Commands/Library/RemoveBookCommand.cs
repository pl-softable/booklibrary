namespace BookLibrary.Core.Commands.Library
{
    using System;
    using MongoDB.Bson;

    public class RemoveBookCommand : ICommand
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
    }
}