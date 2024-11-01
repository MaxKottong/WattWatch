using System.Transactions;
using MongoDB.Bson;

namespace WattWatch.Models; 

public class UsageModel {
    public ObjectId Id { get; set; }
    public string Email { get; set; }
    public double EnergyUsage { get; set; }
    public DateTime Timestamp { get; set; }
}