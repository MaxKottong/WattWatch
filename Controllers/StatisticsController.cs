using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WattWatch.Models;
using WattWatch.Services;
using WattWatch.ViewModel.Dashboard;

namespace WattWatch.Controllers {
    public class StatisticsController : Controller {
        private MongoDbService _mongoDbService;
        private string GetEnergySavingTips(Dictionary<string, double> monthlyEnergyUsage) {
            var sortedMonths = monthlyEnergyUsage.OrderBy(kvp => kvp.Key).ToList();

            if (sortedMonths.Count < 1)
                return null;

            var mostRecentMonth = sortedMonths.Last();
            var recentUsage = mostRecentMonth.Value;
            var averageUsage = monthlyEnergyUsage.Values.Average();

            if (recentUsage > averageUsage * 1.2)
            {
                return "Your energy usage for the most recent month is significantly higher than your average usage. Consider tips like using energy-efficient appliances, unplugging devices not in use, and optimizing heating or cooling systems.";
            }

            return null;
        }

        public IActionResult Index() {
            if (User.Identity.IsAuthenticated) {
                var email = User.Identity.Name;
                var entries = GetUserEntries(email);
                var monthlyEnergyUsage = CalculateMonthlyEnergyUsage(entries);
                var totalEnergyUsed = CalculateTotalEnergyUsed(entries);
                var energySavingTips = GetEnergySavingTips(monthlyEnergyUsage);

                var viewModel = new DashboardViewModel() {
                    UserEntries = entries,
                    MonthlyEnergyUsage = monthlyEnergyUsage,
                    TotalEnergyUsed = totalEnergyUsed,
                    Email = email,
                    EnergySavingTips = energySavingTips
                };

                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public List<UsageModel> GetUserEntries(string email) {
            _mongoDbService = new MongoDbService();

            var entries = _mongoDbService.GetEntriesCollection();

            return entries.AsQueryable().Where(entry => entry.Email == email).ToList();
        }

        private Dictionary<string, double> CalculateMonthlyEnergyUsage(List<UsageModel> entries) {
            return entries
                .GroupBy(entry => new { entry.Timestamp.Year, entry.Timestamp.Month })
                .ToDictionary(
                    g => $"{g.Key.Year}-{g.Key.Month:D2}",
                    g => g.Sum(entry => entry.EnergyUsage)
                );
        }

        private double CalculateTotalEnergyUsed(List<UsageModel> entries) {
            return entries.Sum(entry => entry.EnergyUsage);
        }
    }
}
