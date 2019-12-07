namespace BookLibrary.Core.Events.Library
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class BookRemovedEvent : IEvent
    {
        public BookRemovedEvent()
        {
            CreationDate = DateTime.UtcNow;
            Type = nameof(BookRemovedEvent);
        }

        public Guid BookId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
        public string Type { get; set; }
        [BsonId] 
        public Guid Id { get; set; }
    }
}