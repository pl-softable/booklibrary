namespace BookLibrary.Core.Events
{
    using System;
    using MediatR;
    using MongoDB.Bson;

    public interface IEvent : INotification
    {
        Guid BookId { get; set; }
        Guid Id { get; set; }
        DateTime CreationDate { get; set; }
        string Type { get; set; }
    }
}