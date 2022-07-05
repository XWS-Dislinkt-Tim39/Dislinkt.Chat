using MongoDB.Driver;
using Public_Chat.MongoDB.Attributes;
using Public_Chat.MongoDB.Entities;
using Public_Chat.MongoDB.Factories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Public_Chat.MongoDB.Common
{
    public class MongoDbContext
    {
        private readonly IDatabaseFactory _databaseFactory;
        public MongoDbContext(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public IMongoCollection<T> GetCollection<T>() where T : BaseEntity
        {
            var collectionDefinition = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute))
                as CollectionNameAttribute;

            return GetCollection<T>(collectionDefinition.Name);
        }
        public IMongoCollection<T> GetCollection<T>(string collectionName)
         => _databaseFactory.Create().GetCollection<T>(collectionName);
        public async Task CreateCollection<T>() where T : BaseEntity
        {
            var collectionDefinition = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute))
                as CollectionNameAttribute;

            var db = _databaseFactory.Create();
            var collections = (await db.ListCollectionNamesAsync()).ToList();

            if (collections.Any(c => c == collectionDefinition.Name))
            {
                return;
            }

            await _databaseFactory.Create().CreateCollectionAsync(collectionDefinition.Name);
        }
    }
}
