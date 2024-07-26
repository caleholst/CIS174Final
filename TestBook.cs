using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIS174Final.Models;
using Xunit;

namespace CIS174Final.Tests
{
    public class BookTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void Title_IsRequired()
        {
            // Arrange
            var book = new Book { Title = null };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Title") && v.ErrorMessage == "Title is required");
        }

        [Fact]
        public void Title_CantBeLongerThan100Characters()
        {
            // Arrange
            var book = new Book { Title = new string('A', 101) };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Title") && v.ErrorMessage == "Title can't be longer than 100 characters");
        }

        [Fact]
        public void Title_ContainsInvalidCharacters()
        {
            // Arrange
            var book = new Book { Title = "Invalid@Title!" };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Title") && v.ErrorMessage == "Title can only contain letters, numbers, comma, hyphen, space, question mark, exclamation mark, double quote, and single quote");
        }

        [Fact]
        public void Author_IsRequired()
        {
            // Arrange
            var book = new Book { Author = null };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Author") && v.ErrorMessage == "Author is required");
        }

        [Fact]
        public void Author_CantBeLongerThan50Characters()
        {
            // Arrange
            var book = new Book { Author = new string('A', 51) };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Author") && v.ErrorMessage == "Author can't be longer than 50 characters");
        }

        [Fact]
        public void Author_ContainsInvalidCharacters()
        {
            // Arrange
            var book = new Book { Author = "Invalid@Author!" };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Author") && v.ErrorMessage == "Author can only contain letters, hyphen, double quote, single quote, and spaces");
        }

        [Fact]
        public void PublishYear_MustBeGreaterThanZero()
        {
            // Arrange
            var book = new Book { PublishYear = -1 };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("PublishYear") && v.ErrorMessage == "Publish year must be after 0");
        }

        [Fact]
        public void Rating_MustBeBetween0And10()
        {
            // Arrange
            var book = new Book { Rating = 11 };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Rating") && v.ErrorMessage == "Rating must be between 0 and 10");
        }

        [Fact]
        public void Description_IsRequired()
        {
            // Arrange
            var book = new Book { Description = null };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Description") && v.ErrorMessage == "Description is required");
        }

        [Fact]
        public void Description_CantBeLongerThan1000Characters()
        {
            // Arrange
            var book = new Book { Description = new string('A', 1001) };

            // Act
            var results = ValidateModel(book);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Description") && v.ErrorMessage == "Description can't be longer than 1000 characters");
        }

    }
}