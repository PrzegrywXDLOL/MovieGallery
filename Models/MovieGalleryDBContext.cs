using Microsoft.EntityFrameworkCore;

namespace MovieGallery.Models
{
    public class MovieGalleryDBContext : DbContext
    {
        public MovieGalleryDBContext() { }
        public MovieGalleryDBContext(DbContextOptions<MovieGalleryDBContext> options)
            : base(options) { }

        public DbSet<Movie> Movies { get; set; }
    }
}
