using MovieGallery.Models;

namespace MovieGallery.Services.Interfaces
{
    public interface IMovieService
    {
        Task Add(Movie movie);
        Task Delete(int id);
        Task<Movie> Get(int id);
        Task Update(Movie movie);
        Task<IEnumerable<Movie>> GetAll();
    }
}
