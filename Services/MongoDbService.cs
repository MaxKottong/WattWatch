using MongoDB.Driver;
using WattWatch.Models;

namespace WattWatch.Services; 

public class MongoDbService {
    private readonly IMongoDatabase _database;

    public MongoDbService() {
        var client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase("WattWatch");
    }

    public IMongoCollection<UserModel> GetUserCollection() {
        return _database.GetCollection<UserModel>("users");
    }

    public IMongoCollection<UsageModel> GetEntriesCollection() {
        return _database.GetCollection<UsageModel>("entries");
    }
}