using CIS174Final.Controllers;
using CIS174Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CIS174Final.Tests
{
    public class BookControllerTests
    {
        private readonly Mock<BookContext> _mockContext;
        private readonly BookController _controller;
        private List<Book> _books;

        public BookControllerTests()
        {
            _mockContext = new Mock<BookContext>();
            _controller = new BookController(_mockContext.Object);

            _books = new List<Book>
            {
                new Book { BookId = 1, Title = "The Hobbit", Author = "John Tolkien", PublishYear = 1937, Rating = 5, Description = "How Baggins explored the world" },
                new Book { BookId = 2, Title = "The Hunger Games", Author = "Suzanne Collins", PublishYear = 2008, Rating = 2, Description = "Post-nuclear world while teenagers find there way" }
            };

            var dbSetMock = new Mock<DbSet<Book>>();
            dbSetMock.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(_books.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(_books.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(_books.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(_books.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Books).Returns(dbSetMock.Object);
        }

        //[Fact]
        //public void Index_Returns_ViewResult_With_ListOfBooks()
        //{
        //    // Act
        //    var result = _controller.Index() as ViewResult;

        //    // Assert
        //    Assert.NotNull(result);
        //    var model = Assert.IsAssignableFrom<List<Book>>(result.Model);
        //    Assert.Equal(2, model.Count);
        //}

        [Fact]
        public void Add_Returns_ViewResult_With_NewBook()
        {
            // Act
            var result = _controller.Add() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<Book>(result.Model);
            Assert.Equal(0, model.BookId);
        }

        [Fact]
        public void Edit_Returns_NotFound_For_InvalidId()
        {
            // Act
            var result = _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Returns_ViewResult_With_Book()
        {
            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<Book>(result.Model);
            Assert.Equal(1, model.BookId);
        }

        [Fact]
        public void Delete_Returns_NotFound_For_InvalidId()
        {
            // Act
            var result = _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Returns_ViewResult_With_Book()
        {
            // Act
            var result = _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<Book>(result.Model);
            Assert.Equal(1, model.BookId);
        }
    }
}


