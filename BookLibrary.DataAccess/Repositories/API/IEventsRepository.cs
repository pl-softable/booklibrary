namespace BookLibrary.DataAccess.Repositories.API
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventsRepository<T> where T : class
    {
        Task AddEvent(T @event);
        Task<IEnumerable<T>> GetAllEventsForBook(Guid bookId);
        Task<IEnumerable<T>> GetAllEvents();
        Task<T> GetEvent(Guid id);
    }
}