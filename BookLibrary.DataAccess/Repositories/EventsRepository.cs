namespace BookLibrary.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API;
    using Core.Events;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class EventsRepository<T> : IEventsRepository<T> where T : class, IEvent
    {
        private readonly IMongoDatabase mongoDb;

        public EventsRepository(IConfiguration configuration, MongoClient mongoClient)
        {
            this.mongoDb = mongoClient.GetDatabase(configuration["DatabaseName"]);
        }

        public async Task AddEvent(T @event)
        {
            var collection = this.mongoDb.GetCollection<T>(
                string.Concat(typeof(T).Name, "s"));

            await collection.InsertOneAsync(@event);
        }

        public async Task<IEnumerable<T>> GetAllEventsForBook(Guid bookId)
        {
            var collection = this.mongoDb.GetCollection<T>(string.Concat(typeof(T).Name, "s"))
                .AsQueryable()
                .Where(item => item.BookId == bookId);

            return await collection.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllEvents()
        {
            var collection = this.mongoDb.GetCollection<T>(string.Concat(typeof(T).Name, "s"))
                .AsQueryable();

            return await collection.ToListAsync();
        }

        public async Task<T> GetEvent(Guid id)
        {
            var @event = await this.mongoDb.GetCollection<T>(string.Concat(typeof(T).Name, "s"))
                .FindAsync(item => item.Id == id);

            if (@event == null)
            {
                throw new Exception("Event does not exists.");
            }

            return await @event.FirstOrDefaultAsync();
        }
    }
}