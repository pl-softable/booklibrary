namespace BookLibrary.Core.Commands.Library
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class AddBookCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Author { get; set; }
    }
}