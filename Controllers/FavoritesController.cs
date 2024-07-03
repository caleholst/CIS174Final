using CIS174Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CIS174Final.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly BookContext context;

        public FavoritesController(BookContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            var favoriteIds = Request.Cookies["FavoriteBooks"];
            if (favoriteIds == null)
            {
                return View(new List<Book>());
            }

            var ids = favoriteIds.Split(',').Select(int.Parse).ToList();

            var favoriteBooks = context.Books
                .Where(b => ids.Contains(b.BookId))
                .ToList();

            return View(favoriteBooks);
        }

        public IActionResult Add(int id)
        {
            var favoriteIds = Request.Cookies["FavoriteBooks"];
            List<int> ids;
            if (favoriteIds == null)
            {
                ids = new List<int>();
            }
            else
            {
                ids = favoriteIds.Split(',').Select(int.Parse).ToList();
            }

            if (!ids.Contains(id))
            {
                ids.Add(id);
                Response.Cookies.Append("FavoriteBooks", string.Join(",", ids));
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var favoriteIds = Request.Cookies["FavoriteBooks"];
            if (favoriteIds == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var ids = favoriteIds.Split(',').Select(int.Parse).ToList();

            if (ids.Contains(id))
            {
                ids.Remove(id);
                if (ids.Any())
                {
                    Response.Cookies.Append("FavoriteBooks", string.Join(",", ids));
                }
                else
                {
                    Response.Cookies.Delete("FavoriteBooks");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove()
        {
            Response.Cookies.Delete("FavoriteBooks");
            return RedirectToAction(nameof(Index));
        }
    }
}