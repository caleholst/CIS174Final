using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CIS174Final.Models;
using Xunit;

namespace CIS174Final.Tests
{
    public class BookTests
    {
        private ValidationContext GetValidationContext(object model)
        {
            return new ValidationContext(model, null, null);
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, GetValidationContext(model), validationResults, true);
            return validationResults;
        }

        [Fact]
        public void Book_TitleIsRequired_ShouldFailIfNotProvided()
        {
            var book = new Book();
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Title") && v.ErrorMessage.Contains("Title is required"));
        }

        [Fact]
        public void Book_TitleMaxLength_ShouldFailIfExceeded()
        {
            var book = new Book { Title = new string('a', 101) };
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Title") && v.ErrorMessage.Contains("Title can't be longer than 100 characters"));
        }

        [Fact]
        public void Book_TitleRegex_ShouldFailIfInvalidCharacters()
        {
            var book = new Book { Title = "Invalid@Title!" };
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Title") && v.ErrorMessage.Contains("Title can only contain letters"));
        }

        [Fact]
        public void Book_AuthorIsRequired_ShouldFailIfNotProvided()
        {
            var book = new Book();
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Author") && v.ErrorMessage.Contains("Author is required"));
        }

        [Fact]
        public void Book_AuthorMaxLength_ShouldFailIfExceeded()
        {
            var book = new Book { Author = new string('a', 51) };
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Author") && v.ErrorMessage.Contains("Author can't be longer than 50 characters"));
        }

        [Fact]
        public void Book_AuthorRegex_ShouldFailIfInvalidCharacters()
        {
            var book = new Book { Author = "Invalid@Author!" };
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Author") && v.ErrorMessage.Contains("Author can only contain letters"));
        }

        [Fact]
        public void Book_PublishYearRange_ShouldFailIfNegative()
        {
            var book = new Book { PublishYear = -1 };
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("PublishYear") && v.ErrorMessage.Contains("Publish year must be after 0"));
        }

        [Fact]
        public void Book_RatingRange_ShouldFailIfOutOfRange()
        {
            var book = new Book { Rating = 11 };
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Rating") && v.ErrorMessage.Contains("Rating must be between 0 and 10"));
        }

        [Fact]
        public void Book_DescriptionIsRequired_ShouldFailIfNotProvided()
        {
            var book = new Book();
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Description") && v.ErrorMessage.Contains("Description is required"));
        }

        [Fact]
        public void Book_DescriptionMaxLength_ShouldFailIfExceeded()
        {
            var book = new Book { Description = new string('a', 1001) };
            var results = ValidateModel(book);
            Assert.Contains(results, v => v.MemberNames.Contains("Description") && v.ErrorMessage.Contains("Description can't be longer than 1000 characters"));
        }
    }

}