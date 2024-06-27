namespace CIS174Final.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }
        public float Rating { get; set; }
        public string Description { get; set; }

        public string Slug => Title?.Replace(' ', '-').ToLower() + '-' + Author?.ToString();

        public ICollection<Review> Reviews { get; set; }
    }

    public class Review
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }

        public Book Book { get; set; }
    }
}