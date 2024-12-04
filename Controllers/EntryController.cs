using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WattWatch.Models;
using WattWatch.Services;

namespace WattWatch.Controllers {
    public class EntryController : Controller {
        private readonly MongoDbService _mongoDbService;

        public EntryController() {
            _mongoDbService = new MongoDbService();
        }

        public IActionResult CreateEntry(UsageModel entry) {
            if (entry == null || entry.EnergyUsage <= 0) {
                return BadRequest("Invalid entry data.");
            }

            var email = User.Identity.Name;
            entry.Email = email;

            _mongoDbService.GetEntriesCollection().InsertOne(entry);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult UpdateEntry(UsageModel entry) {
            if (entry == null) {
                return BadRequest("Entry data is null.");
            }

            if (entry.EnergyUsage <= 0) {
                return BadRequest("Energy usage must be greater than zero.");
            }

            if (entry.Timestamp == default(DateTime)) {
                return BadRequest("Invalid or missing timestamp.");
            }

            var email = User.Identity.Name;

            if (!ObjectId.TryParse(entry.Id.ToString(), out ObjectId objectId)) {
                return BadRequest("Invalid ObjectId format.");
            }

            var filter = Builders<UsageModel>.Filter.Where(e => e.Id == objectId && e.Email == email);
            var update = Builders<UsageModel>.Update
                .Set(e => e.EnergyUsage, entry.EnergyUsage)
                .Set(e => e.Timestamp, entry.Timestamp);

            var result = _mongoDbService.GetEntriesCollection().UpdateOne(filter, update);

            if (result.MatchedCount == 0) {
                return NotFound("Entry not found or unauthorized.");
            }

            return RedirectToAction("Index", "Dashboard");
        }


        public IActionResult DeleteEntry(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return BadRequest("Invalid entry ID.");
            }

            var email = User.Identity.Name;

            if (!ObjectId.TryParse(id, out ObjectId objectId)) {
                return BadRequest("Invalid ObjectId format.");
            }

            var filter = Builders<UsageModel>.Filter.Where(e => e.Id == objectId && e.Email == email);

            var result = _mongoDbService.GetEntriesCollection().DeleteOne(filter);

            if (result.DeletedCount == 0) {
                return NotFound("Entry not found or unauthorized.");
            }

            return Ok();
        }


    }
}
