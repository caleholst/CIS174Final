using Microsoft.AspNetCore.Mvc;
using CIS174Final.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CIS174Final.Controllers
{
    public class BookController : Controller
    {
        private BookContext context { get; set; }

        public BookController(BookContext ctx) => context = ctx;

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                ViewBag.Action = "Add";
                return View("Edit", new Book());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Action = "Edit";
                var book = context.Books.Find(id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            try
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
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var book = context.Books.Find(id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            try
            {
                context.Books.Remove(book);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Review(int id)
        {
            try
            {
                var book = context.Books.Include(b => b.Reviews).FirstOrDefault(b => b.BookId == id);
                if (book == null)
                {
                    return NotFound();
                }

                ViewBag.AverageRating = book.Reviews.Any() ? book.Reviews.Average(r => r.Rating) : 0.0f;
                return View(book);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Review(int id, int rating)
        {
            try
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
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Books(string sortOrder)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                IQueryable<Book> booksQuery = context.Books.Include(b => b.Reviews);

                switch (sortOrder)
                {
                    case "newest":
                        booksQuery = booksQuery.OrderByDescending(b => b.BookId);
                        break;
                    case "oldest":
                        booksQuery = booksQuery.OrderBy(b => b.BookId);
                        break;
                    case "highest":
                        booksQuery = booksQuery.OrderByDescending(b => b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0.0);
                        break;
                    default:
                        booksQuery = booksQuery.OrderBy(b => b.Title);
                        break;
                }

                var books = booksQuery.ToList();
                return View("~/Views/Home/Books.cshtml", books);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }
    }
}