using Microsoft.AspNetCore.Mvc;

namespace WattWatch.Controllers {
    public class AccountController : Controller {
        public IActionResult Index() {
            return View("Login");
        }

        public IActionResult Register() {
            return View();
        }

        public IActionResult Login() {
            return View();
        }
    }
}
