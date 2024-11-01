using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WattWatch.Models;
using WattWatch.Services;

namespace WattWatch.Controllers {
    public class AccountController : Controller {
        private readonly MongoDbService _mongoDbService;

        public IActionResult Index() {
            return View("Login");
        }

        public IActionResult Register() {
            return View();
        }

        public IActionResult Login(UserModel model) {
            var user = _mongoDbService.Authenticate(model.Email, model.Password);

            if (user != null) {
                HttpContext.Session.SetString("UserId", model.Id);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model) {
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
