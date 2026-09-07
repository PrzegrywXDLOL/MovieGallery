using Microsoft.EntityFrameworkCore;
using MovieGallery.Models;
using MovieGallery.Services.Interfaces;

namespace MovieGallery.Services
{
    public class ReviewDBService: IReviewDBService
    {
        private readonly MovieGalleryDBContext _db;
        public ReviewDBService(MovieGalleryDBContext db)
        {
            _db = db;
        }
        public async Task Add(Review review)
        {
            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Review>> GetReviews(int movieId)
        {
            return await _db.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }
    }
}
