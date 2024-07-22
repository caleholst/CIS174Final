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