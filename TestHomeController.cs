using CIS174Final.Controllers;
using CIS174Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace CIS174Final.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<BookContext> _mockContext;
        private readonly HomeController _controller;
        private List<Book> _books;

        public HomeControllerTests()
        {
            _mockContext = new Mock<BookContext>();
            _controller = new HomeController(_mockContext.Object);

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

        [Fact]
        public void Index_Returns_ViewResult_With_ListOfBooks()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<Book>>(result.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Books_Returns_ViewResult_With_ListOfBooks()
        {
            // Act
            var result = _controller.Books() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<Book>>(result.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Error_Returns_ViewResult_With_ErrorViewModel()
        {
            // Arrange
            var errorMessage = "Test error message";
            var traceId = Activity.Current?.Id ?? "test_trace_id";
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.Error(errorMessage) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(result.Model);
            Assert.Equal(errorMessage, model.Message);
            Assert.Equal(traceId, model.RequestId);
            Assert.True(model.ShowRequestId);
        }
    }
}
