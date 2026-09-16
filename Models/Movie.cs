using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieGallery.Models
{
    public class Movie
    {
        public static List<Movie> list { get; set; } = new List<Movie>();
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public byte[]? Poster { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [NotMapped]
        [JsonIgnore]
        public List<string> SelectedGenres { get; set; } = new List<string>();
    }
}