using Microsoft.AspNetCore.Mvc;
using CIS174Final.Models;
using Microsoft.EntityFrameworkCore;

namespace CIS174Final.Controllers;
public class BookController : Controller
{
    private BookContext context { get; set; }

    public BookController(BookContext ctx) => context = ctx;

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "Add";
        return View("Edit", new Book());
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        ViewBag.Action = "Edit";
        var book = context.Books.Find(id);
        return View(book);
    }

    [HttpPost]
    public IActionResult Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            if (book.BookId == 0)
                context.Books.Add(book);
            else
                context.Books.Update(book);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.Action = (book.BookId == 0) ? "Add" : "Edit";
            return View(book);
        }
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var book = context.Books.Find(id);
        return View(book);
    }

    [HttpPost]
    public IActionResult Delete(Book book)
    {
        context.Books.Remove(book);
        context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult Review(int id)
    {
        var book = context.Books.Include(b => b.Reviews).FirstOrDefault(b => b.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        ViewBag.AverageRating = book.Reviews.Any() ? book.Reviews.Average(r => r.Rating) : 0.0f;
        return View(book);
    }

    [HttpPost]
    public IActionResult Review(int id, int rating)
    {
        var book = context.Books.Include(b => b.Reviews).FirstOrDefault(b => b.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        var review = new Review
        {
            Rating = rating,
            Book = book
        };

        context.Reviews.Add(review);
        context.SaveChanges();

        // Update book's average rating
        ViewBag.AverageRating = book.Reviews.Any() ? book.Reviews.Average(r => r.Rating) : 0.0;

        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpGet]
    public IActionResult Books(string sortOrder)
    {
        ViewBag.CurrentSort = sortOrder;
        IQueryable<Book> booksQuery = context.Books.Include(b => b.Reviews);

        switch (sortOrder)
        {
            case "newest":
                booksQuery = booksQuery.OrderByDescending(b => b.BookId); // Assuming that the Order the ID's are in is the Order they where added.
                break;
            case "oldest":
                booksQuery = booksQuery.OrderBy(b => b.BookId);
                break;
            case "highest":
                booksQuery = booksQuery.OrderByDescending(b => b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0.0);
                break;
            default:
                booksQuery = booksQuery.OrderBy(b => b.Title); // I dont think this is doing anything ? 
                break;
        }

        var books = booksQuery.ToList(); 

        return View("~/Views/Home/Books.cshtml", books);
    }
}



