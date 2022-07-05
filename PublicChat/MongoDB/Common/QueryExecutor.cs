
using MongoDB.Driver;
using Public_Chat.MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Public_Chat.MongoDB.Common
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly MongoDbContext _mongoDbContext;
        public QueryExecutor(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        public async Task CreateAsync<T>(T t) where T : BaseEntity
         => await _mongoDbContext.GetCollection<T>().InsertOneAsync(t);

        public async Task DeleteByIdAsync<T>(FilterDefinition<T> filterDefinition) where T : BaseEntity
        => await _mongoDbContext.GetCollection<T>().DeleteOneAsync(filterDefinition);

        public async Task<IReadOnlyCollection<T>> FindAsync<T>(FilterDefinition<T> filterDefinition) where T : BaseEntity
       => await _mongoDbContext.GetCollection<T>().FindAsync(filterDefinition).Result.ToListAsync();

        public async Task<T> FindByIdAsync<T>(Guid id) where T : BaseEntity
        => await _mongoDbContext.GetCollection<T>().Find(b => b.Id == id).FirstOrDefaultAsync();
        public async Task<IReadOnlyCollection<T>> GetAll<T>() where T : BaseEntity
            => await _mongoDbContext.GetCollection<T>().Find(_ => true).ToListAsync();

        public async Task UpdateAsync<T>(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition) where T : BaseEntity
        => await _mongoDbContext.GetCollection<T>().UpdateOneAsync(filterDefinition, updateDefinition);

    }
}
