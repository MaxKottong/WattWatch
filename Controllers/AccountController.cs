using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WattWatch.Models;
using WattWatch.Services;

namespace WattWatch.Controllers {
    public class AccountController : Controller {
        private MongoDbService _mongoDbService;

        public IActionResult Index() {
            return View("Login");
        }

        public IActionResult Login(UserModel model) {
            _mongoDbService = new MongoDbService();

            var user = _mongoDbService.Authenticate(model.Email, model.Password);

            if (user != null) {
                HttpContext.Session.SetString("UserId", user.Id.ToString());

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model) {
            _mongoDbService = new MongoDbService();

            if (!ModelState.IsValid) {
                return View(model);
            }

            if (model.Password != model.PasswordConfirm) {
                ModelState.AddModelError("", "Passwords do not match.");
                return View(model);
            }

            var user = new UserModel {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            };

            _mongoDbService.CreateUser(user);

            return RedirectToAction("Login", "Account");
        }
    }
}
