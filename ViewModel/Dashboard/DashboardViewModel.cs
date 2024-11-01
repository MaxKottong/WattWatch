using WattWatch.Models;

namespace WattWatch.ViewModel.Dashboard; 

public class DashboardViewModel {
    public List<UsageModel> UserEntries { get; set; }
    public bool HasEntries => UserEntries != null && UserEntries.Count > 0;
    public string Email { get; set; }
    public int TotalEntriesCount => UserEntries?.Count ?? 0;
    public double TotalEnergyUsage => UserEntries?.Sum(entry => entry.EnergyUsage) ?? 0;
    public DateTime? LastEntryDate => UserEntries?.OrderByDescending(entry => entry.Timestamp).FirstOrDefault()?.Timestamp;
}