namespace MovieGallery.Models
{
    public class Movie
    {
        public static List<Movie> list { get; set; } = new List<Movie>();
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public byte[]? Poster { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
    }
}