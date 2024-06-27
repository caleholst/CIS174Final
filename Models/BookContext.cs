using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace CIS174Final.Models;

public class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options)
    { }
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                BookId = 1,
                Title = "100 Years of Solitude",
                Author = "Gabriel Garcia Marquez",
                PublishYear = 1967,
                Rating = 7,
                Description = "Good book",
            },
            new Book
            {
                BookId = 2,
                Title = "Flowers for Algernon",
                Author = "Daniel Keyes",
                PublishYear = 1959,
                Rating = 8.5F,
                Description = "Great book",
             },
            new Book
             {
                BookId = 3,
                Title = "The Catcher in the Rye",
                Author = "J. D. Salinger",
                PublishYear = 1945,
                Rating = 4,
                Description = "Okay book",
             }
            );
    }
}
