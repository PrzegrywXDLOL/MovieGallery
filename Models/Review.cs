using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieGallery.Models
{
    public class Review
    {
        public static List<Review> list { get; set; } = new List<Review>();
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string ReviewerName { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }

    }
}
