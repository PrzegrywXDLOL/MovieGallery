using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieGallery.Data;
using MovieGallery.Models;

namespace MovieGallery.Controllers.Api
{
    [Route("api/Movies")]
    [ApiController]
    public class MoviesApiController : ControllerBase
    {
        private readonly MovieGalleryDBContext _context;

        public MoviesApiController(MovieGalleryDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies
                            .Include(movie => movie.Reviews)
                            .ToListAsync();
        }
    }
}
