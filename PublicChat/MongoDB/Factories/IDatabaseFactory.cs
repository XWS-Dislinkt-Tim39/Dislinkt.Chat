using MongoDB.Driver;

namespace Public_Chat.MongoDB.Factories
{
    public interface IDatabaseFactory
    {
        IMongoDatabase Create();
    }
}
