using MongoDB.Driver;
using WattWatch.Models;

namespace WattWatch.Services; 

public class MongoDbService {
    private readonly IMongoCollection<UserModel> _users;

    public MongoDbService() {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("YourDatabaseName");
        _users = database.GetCollection<UserModel>("Users");
    }

    public UserModel Authenticate(string email, string password) {
        // Find user with matching username and password (use hashed passwords in production).
        return _users.Find(user => user.Email == email && user.Password == password).FirstOrDefault();
    }
}