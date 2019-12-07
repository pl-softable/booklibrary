namespace BookLibrary.Core.Events
{
    using System.Threading.Tasks;

    public interface IEventBus
    {
        Task Publish<TEvent>(params TEvent[] events) where TEvent : IEvent;
    }
}