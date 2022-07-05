using MongoDB.Driver;
using Public_Chat.MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Public_Chat.MongoDB.Common
{
    public interface IQueryExecutor
    {
        Task<T> FindByIdAsync<T>(Guid id) where T : BaseEntity;
        Task<IReadOnlyCollection<T>> FindAsync<T>(FilterDefinition<T> filterDefinition) where T : BaseEntity;
        Task CreateAsync<T>(T t) where T : BaseEntity;
        Task DeleteByIdAsync<T>(FilterDefinition<T> filterDefinition) where T : BaseEntity;
        Task UpdateAsync<T>(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition) where T : BaseEntity;
        Task<IReadOnlyCollection<T>> GetAll<T>() where T : BaseEntity;
    }
}
