using System.ComponentModel.DataAnnotations;

namespace CIS174Final.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9,?!\- ""']+$", ErrorMessage = "Title can only contain letters, numbers, comma, hyphen, space, question mark, exclamation mark, double quote, and single quote")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(50, ErrorMessage = "Author can't be longer than 50 characters")]
        [RegularExpression(@"^[a-zA-Z\-'""]+( [a-zA-Z\-'""]+)*$", ErrorMessage = "Author can only contain letters, hyphen, double quote, single quote, and spaces")]
        public string Author { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Publish year must be after 0")]
        public int PublishYear { get; set; }

        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        public float Rating { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters")]
        public string Description { get; set; }

        public string Slug => Title?.Replace(' ', '-').ToLower() + '-' + Author?.ToString();

        public ICollection<Review> Reviews { get; set; }
    }

    public class Review
    {
        public int ReviewId { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public int Rating { get; set; }

        [Required]
        public Book Book { get; set; }
    }
}