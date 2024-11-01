using MongoDB.Driver;
using WattWatch.Models;

namespace WattWatch.Services; 

public class UsageService {
    private readonly IMongoCollection<UsageModel> _entriesCollection;

    public UsageService(IMongoDatabase database) {
        _entriesCollection = database.GetCollection<UsageModel>("entries");
    }

    public bool HasEntries(string email) {
        return _entriesCollection.AsQueryable().Any(entry => entry.Email == email);
    }
}