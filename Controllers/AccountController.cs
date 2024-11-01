using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WattWatch.Models;
using WattWatch.Services;

namespace WattWatch.Controllers {
    public class AccountController : Controller {
        private readonly MongoDbService _mongoDbService;

        public AccountController() {
            _mongoDbService = new MongoDbService();
        }

        public IActionResult Index() {
            return View("Login");
        }

        public IActionResult Register() {
            return View();
        }

        public IActionResult Login(UserModel model) {
            var user = Authenticate(model.Email, model.Password);

            if (user != null) {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("FirstName", user.FirstName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal).Wait();

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        public IActionResult Logout() {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction("Index", "Home");
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

            CreateUser(user);

            return RedirectToAction("Login", "Account");
        }

        public UserModel Authenticate(string email, string password) {
            var users = _mongoDbService.GetUserCollection();

            return users.Find(user => user.Email == email && user.Password == password).FirstOrDefault();
        }

        public void CreateUser(UserModel user) {
            var users = _mongoDbService.GetUserCollection();

            var existingUser = users.Find(u => u.Email == user.Email).FirstOrDefault();

            if (existingUser != null) {
                throw new InvalidOperationException("Email is already in use.");
            }

            users.InsertOne(user);
        }
    }
}
