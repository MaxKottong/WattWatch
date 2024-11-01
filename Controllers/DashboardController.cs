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

                var viewModel = new DashboardViewModel() {
                    UserEntries = entries,
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
    }
}
