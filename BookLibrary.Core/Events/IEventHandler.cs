namespace BookLibrary.Core.Events
{
    using MediatR;

    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
        where TEvent : IEvent
    {
    }
}