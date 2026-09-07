using MovieGallery.Models;

namespace MovieGallery.Services.Interfaces
{
    public interface IReviewService
    {
        Task Add(Review review);
        Task<IEnumerable<Review>> GetReviews(int movieId);
    }
}
