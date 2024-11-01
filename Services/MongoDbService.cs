using MongoDB.Driver;
using WattWatch.Models;

namespace WattWatch.Services; 

public class MongoDbService {
    private readonly IMongoCollection<UserModel> _users;

    public MongoDbService() {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("WattWatch");
        _users = database.GetCollection<UserModel>("users");
    }

    public UserModel Authenticate(string email, string password) {

        return _users.Find(user => user.Email == email && user.Password == password).FirstOrDefault();
    }

    public void CreateUser(UserModel user) {
        var existingUser = _users.Find(u => u.Email == user.Email).FirstOrDefault();

        if (existingUser != null) {
            throw new InvalidOperationException("Email is already in use.");
        }

        _users.InsertOne(user);
    }
}