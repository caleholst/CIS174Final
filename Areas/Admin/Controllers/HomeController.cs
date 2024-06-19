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
}