using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WattWatch.Models; 

public class UserModel {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

}