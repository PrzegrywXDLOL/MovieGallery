using MovieGallery.Models;
namespace MovieGallery.Services.Interfaces
{
    public interface IMovieDBService
    {
        Task Add(Movie movie);
        Task Delete(int id);
        Task<Movie> Get(int id);
        Task Update(Movie movie);
        Task<IEnumerable<Movie>> GetTitle();
        Task<byte[]?> Poster(int id);
        Task<Movie> Details(int id);
    }
}
