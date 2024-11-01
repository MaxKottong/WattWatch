using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WattWatch.Models;
using WattWatch.Services;
using WattWatch.ViewModel.Dashboard;

namespace WattWatch.Controllers {
    public class DashboardController : Controller {
        private MongoDbService _mongoDbService;

        public IActionResult Index() {
            if (User.Identity.IsAuthenticated) {
                var email = User.Identity.Name;
                var entries = GetUserEntries(email);
                var monthlyEnergyUsage = CalculateMonthlyEnergyUsage(entries);
                var totalEnergyUsed = CalculateTotalEnergyUsed(entries);

                var viewModel = new DashboardViewModel() {
                    UserEntries = entries,
                    MonthlyEnergyUsage = monthlyEnergyUsage,
                    TotalEnergyUsed = totalEnergyUsed,
                    Email = email
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
