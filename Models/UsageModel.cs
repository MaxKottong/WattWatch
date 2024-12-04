using System.Transactions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WattWatch.Models; 

public class UsageModel {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string Email { get; set; }
    public double EnergyUsage { get; set; }
    public DateTime Timestamp { get; set; }
}