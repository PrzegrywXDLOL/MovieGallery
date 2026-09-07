using MovieGallery.Models;
using MovieGallery.Services.Interfaces;

namespace MovieGallery.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieDBService _db;
        public MovieService(IMovieDBService db)
        {
            _db = db;
        }

        private Movie Upper(Movie movie)
        {
            movie.Title = movie.Title.ToUpper();
            movie.Director = movie.Director.ToUpper();
            movie.Cast = movie.Cast.ToUpper();
            return movie;
        }

        public async Task Add(Movie movie)
        {
            movie = Upper(movie);
            await _db.Add(movie);
        }

        public async Task Delete(int id)
        {
            await _db.Delete(id);
        }

        public async Task<Movie> Get(int id)
        {
            return await _db.Get(id);
        }
        public async Task Update(Movie movie)
        {
            movie = Upper(movie);
            await _db.Update(movie);
        }
        public async Task<IEnumerable<Movie>> GetTitle()
        {
            return await _db.GetTitle();
        }
        public async Task<byte[]?> Poster(int id)
        {
            return await _db.Poster(id);
        }
        public async Task<Movie> Details(int id)
        {
            return await _db.Details(id);
        }
    }
}
