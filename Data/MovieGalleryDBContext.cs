using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieGallery.Models;

namespace MovieGallery.Data
{
    public class MovieGalleryDBContext : IdentityDbContext<IdentityUser>
    {
        public MovieGalleryDBContext() { }
        public MovieGalleryDBContext(DbContextOptions<MovieGalleryDBContext> options)
            : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
