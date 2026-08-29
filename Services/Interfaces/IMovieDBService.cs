using MovieGallery.Models;
namespace MovieGallery.Services.Interfaces
{
    public interface IMovieDBService
    {
        Task Add(Movie movie);
        Task Delete(int id);
        Task<Movie> Get(int id);
        Task<IEnumerable<Movie>> GetAll();
        Task Update(Movie movie);
    }
}
