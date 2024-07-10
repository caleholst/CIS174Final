using CIS174Final.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace CIS174Final.Controllers;

public class HomeController : Controller
{
    private BookContext context { get; set; }

    public HomeController(BookContext ctx) => context = ctx;

    public IActionResult Index()
    {
        try
        {
            var books = context.Books.ToList();
            return View(books);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", new { message = ex.Message });
        }
    }

    public IActionResult Books()
    {
        try
        {
            var books = context.Books.ToList();
            return View(books);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", new { message = ex.Message });
        }
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
