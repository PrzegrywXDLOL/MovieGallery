using MovieGallery.Models;
using MovieGallery.Services.Interfaces;

namespace MovieGallery.Services
{
    public class ReviewService: IReviewService
    {
        private readonly IReviewDBService _db;
        public ReviewService(IReviewDBService db)
        {
            _db = db;
        }

        public async Task Add(Review review)
        {
            await _db.Add(review);
        }
        public async Task<IEnumerable<Review>> GetReviews(int movieId)
        {
            return await _db.GetReviews(movieId);
        }
    }
}
