using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CIS174Final.Models;
using Microsoft.AspNetCore.Http;

namespace CIS174Final.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]  // Restrict access to admin role
    public class BookController : Controller
    {
        private BookContext context { get; set; }

        public BookController(BookContext ctx)
        {
            context = ctx;
        }
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        private IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Home", new { area = "" });
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (!IsAdmin()) return RedirectToLogin();

            ViewBag.Action = "Add";
            return View("Edit", new Book());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            if (!IsAdmin()) return RedirectToLogin();

            ViewBag.Action = "Edit";
            var book = context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Action = "Edit";
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!IsAdmin()) return RedirectToLogin();

            if (ModelState.IsValid)
            {
                if (book.BookId == 0)
                {
                    context.Books.Add(book);
                }
                else
                {
                    context.Books.Update(book);
                }
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Action = (book.BookId == 0) ? "Add" : "Edit";
            return View(book);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToLogin();

            var book = context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            if (!IsAdmin()) return RedirectToLogin();

            context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
