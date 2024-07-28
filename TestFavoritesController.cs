using CIS174Final.Controllers;
using CIS174Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CIS174Final.Tests
{
    public class FavoritesControllerTests
    {
        private readonly Mock<BookContext> _mockContext;
        private readonly FavoritesController _controller;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<IRequestCookieCollection> _mockRequestCookies;
        private readonly Mock<IResponseCookies> _mockResponseCookies;
        private List<Book> _books;

        public FavoritesControllerTests()
        {
            _mockContext = new Mock<BookContext>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockRequestCookies = new Mock<IRequestCookieCollection>();
            _mockResponseCookies = new Mock<IResponseCookies>();
            _controller = new FavoritesController(_mockContext.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

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
        public void Index_Returns_ViewResult_With_ListOfFavoriteBooks()
        {
            // Arrange
            _mockRequestCookies.Setup(c => c["FavoriteBooks"]).Returns("1,2");
            _mockHttpContext.Setup(c => c.Request.Cookies).Returns(_mockRequestCookies.Object);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<Book>>(result.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Index_Returns_ViewResult_With_EmptyList_When_NoFavorites()
        {
            // Arrange
            _mockRequestCookies.Setup(c => c["FavoriteBooks"]).Returns((string)null);
            _mockHttpContext.Setup(c => c.Request.Cookies).Returns(_mockRequestCookies.Object);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<Book>>(result.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Add_Adds_BookId_To_Cookies_And_Redirects_To_Index()
        {
            // Arrange
            _mockRequestCookies.Setup(c => c["FavoriteBooks"]).Returns("1");
            _mockHttpContext.Setup(c => c.Request.Cookies).Returns(_mockRequestCookies.Object);
            _mockHttpContext.Setup(c => c.Response.Cookies).Returns(_mockResponseCookies.Object);

            // Act
            var result = _controller.Add(2) as RedirectToActionResult;

            // Assert
            _mockResponseCookies.Verify(c => c.Append("FavoriteBooks", "1,2"), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Remove_Removes_BookId_From_Cookies_And_Redirects_To_Index()
        {
            // Arrange
            _mockRequestCookies.Setup(c => c["FavoriteBooks"]).Returns("1,2");
            _mockHttpContext.Setup(c => c.Request.Cookies).Returns(_mockRequestCookies.Object);
            _mockHttpContext.Setup(c => c.Response.Cookies).Returns(_mockResponseCookies.Object);

            // Act
            var result = _controller.Remove(1) as RedirectToActionResult;

            // Assert
            _mockResponseCookies.Verify(c => c.Append("FavoriteBooks", "2"), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Remove_Deletes_Cookies_When_No_Favorites_Left_And_Redirects_To_Index()
        {
            // Arrange
            _mockRequestCookies.Setup(c => c["FavoriteBooks"]).Returns("1");
            _mockHttpContext.Setup(c => c.Request.Cookies).Returns(_mockRequestCookies.Object);
            _mockHttpContext.Setup(c => c.Response.Cookies).Returns(_mockResponseCookies.Object);

            // Act
            var result = _controller.Remove(1) as RedirectToActionResult;

            // Assert
            _mockResponseCookies.Verify(c => c.Delete("FavoriteBooks"), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Remove_Post_Deletes_Cookies_And_Redirects_To_Index()
        {
            // Arrange
            _mockHttpContext.Setup(c => c.Response.Cookies).Returns(_mockResponseCookies.Object);

            // Act
            var result = _controller.Remove() as RedirectToActionResult;

            // Assert
            _mockResponseCookies.Verify(c => c.Delete("FavoriteBooks"), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}

