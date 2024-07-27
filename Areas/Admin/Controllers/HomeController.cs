using CIS174Final.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIS174Final.Areas.Admin.Controllers;
[Area("Admin")]
public class HomeController : Controller
{
    private BookContext context { get; set; }

    public HomeController(BookContext ctx) => context = ctx;

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("IsAdmin") == "true";
    }

    private IActionResult RedirectToLogin()
    {
        return RedirectAction("Login, "Home", new { area = ""});
    )

    public IActionResult Index()
    {
        if (!isIAdmin()) return RedirectToLogin();

        var books = context.Books.ToList();
        return View(books);
    }

    public IActionResult Index()
    {
        if (!IsAdmin()) return RedirectToLogin();

        var books = context.Books.ToList();
        return View(books);
    }

    public IActionResult Books()
    {
        if (!IsAdmin()) return RedirectToLogin();
        
        var books = context.Books.ToList();
        return View(books);
        }
    }
}
