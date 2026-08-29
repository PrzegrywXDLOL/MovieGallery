using Microsoft.EntityFrameworkCore;
using MovieGallery.Models;
using MovieGallery.Services.Interfaces;

namespace MovieGallery.Services
{
    public class MovieDBService : IMovieDBService
    {
        private readonly MovieGalleryDBContext _db;
        public MovieDBService(MovieGalleryDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _db.Movies.ToListAsync();
        }

        public async Task<Movie> Get(int id)
        {
            return await _db.Movies.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task Update(Movie movie)
        {
            var tempMovie = await Get(movie.Id);
            if (tempMovie != null)
            {
                tempMovie.Title = movie.Title;
                tempMovie.Director = movie.Director;
                tempMovie.ReleaseDate = movie.ReleaseDate;
                await _db.SaveChangesAsync();
            }
        }
        public async Task Add(Movie movie)
        {
            await _db.Movies.AddAsync(movie);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var movie = await Get(id);
            if (movie != null)
            {
                _db.Movies.Remove(movie);
                await _db.SaveChangesAsync();
            }
        }
    }
}
