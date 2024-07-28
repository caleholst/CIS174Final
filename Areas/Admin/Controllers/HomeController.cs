using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CIS174Final.Models;

namespace CIS174Final.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly BookContext context;

        public HomeController(BookContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            var books = context.Books.ToList();
            return View(books);
        }
    }
}
