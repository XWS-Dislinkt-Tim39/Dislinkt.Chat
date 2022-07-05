using MongoDB.Driver;

namespace Public_Chat.MongoDB.Factories
{
    public class DatabaseFactory : IDatabaseFactory
    {
        public IMongoDatabase Create()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            return mongoClient.GetDatabase("ChatDB");
        }
    }
}
