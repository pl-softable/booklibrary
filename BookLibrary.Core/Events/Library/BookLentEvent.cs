namespace BookLibrary.Core.Events.Library
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class BookLentEvent : IEvent
    {
        public DateTime LentStart { get; set; }
        public DateTime LentEnd { get; set; }
        public Guid BookId { get; set; }
        public Guid PersonId { get; set; }

        [BsonId] 
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public string Type { get; set; }
    }
}