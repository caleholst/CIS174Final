using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CIS174Final.Models;
using System.Diagnostics;

namespace CIS174Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookContext _context;

        public HomeController(BookContext ctx)
        {
            _context = ctx;
        }

        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        public IActionResult Books()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Simple password for us to have
            const string adminUsername = "admin";
            const string adminPassword = "password";

            if (username == adminUsername && password == adminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            ViewBag.Error = "Invalid username or password";
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
