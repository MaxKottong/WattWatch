using WattWatch.Models;

namespace WattWatch.ViewModel.Dashboard; 

public class DashboardViewModel {
    public List<UsageModel> UserEntries { get; set; }
    public bool HasEntries => UserEntries != null && UserEntries.Count > 0;
    public string Email { get; set; }
    public double TotalEnergyUsed { get; set; }
    public Dictionary<string, double> MonthlyEnergyUsage { get; set; }
}