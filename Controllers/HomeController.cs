using CIS174Final.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIS174Final.Controllers;
public class HomeController : Controller
{
    private BookContext context { get; set; }

    public HomeController(BookContext ctx) => context = ctx;

    public IActionResult Index()
    {
        var books = context.Books.ToList();
        return View(books);
    }

}