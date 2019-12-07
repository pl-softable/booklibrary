namespace BookLibrary.Core.Events.Library
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class BookCreatedEvent : IEvent
    {
        [BsonId] 
        public Guid Id { get; set; }

        public int Quantity { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string Type { get; set; }
        public Guid BookId { get; set; }
    }
}