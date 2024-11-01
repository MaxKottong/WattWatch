using Microsoft.AspNetCore.Mvc;
using WattWatch.Models;
using WattWatch.Services;

namespace WattWatch.Controllers {
    public class EntryController : Controller {
        private readonly MongoDbService _mongoDbService;

        public EntryController() {
            _mongoDbService = new MongoDbService();
        }

        public IActionResult CreateEntry([FromBody] UsageModel entry) {
            if (entry == null || entry.EnergyUsage <= 0) {
                return BadRequest("Invalid entry data.");
            }

            var email = User.Identity.Name;
            entry.Email = email;

            _mongoDbService.GetEntriesCollection().InsertOne(entry);

            return Ok();
        }
    }
}
