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
        public async Task<IEnumerable<Movie>> GetTitle()
        {
            return await _db.Movies
                .Select(m => new Movie
                {
                    Id = m.Id,
                    Title = m.Title
                })
                .ToListAsync();
        }

        public async Task<Movie> Get(int id)
        {
            return await _db.Movies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<byte[]?> Poster(int id)
        {
            var poster = await _db.Movies
                .Where(m => m.Id == id)
                .Select(m => m.Poster)
                .FirstOrDefaultAsync();
            return poster;
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

        public async Task<Movie> Details(int id)
        {
            var movie = await _db.Movies
                .Where(m => m.Id == id)
                .Select(m => new Movie
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    ReleaseDate = m.ReleaseDate,
                    Genre = m.Genre,
                    Description = m.Description
                })
                .FirstOrDefaultAsync();
            return movie;
        }
    }
}
