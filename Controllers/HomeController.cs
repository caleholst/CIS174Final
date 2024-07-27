using CIS174Final.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIS174Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookContext context;

        public HomeController(BookContext ctx) => context = ctx;

        public IActionResult Index()
        {
            var books = context.Books.ToList();
            return View(books);
        }

        public IActionResult Books()
        {
            var books = context.Books.ToList();
            return View(books);
        }

        public IActionResult Login()
        {
            return view();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            //making this simple for our use only, if need be you can make changes :)
            const string adminUsername = "admin";
            const string adminPassowrd = "password";

            if (username == adminUsername && password == adminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Home", new { area = "Admin"});
            }

            Viewbag.Error = "Invalid username or password
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Index");
        }

        public IActionResult Error(string message)
        {
            var model = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(model);
        }
    }
}
