using Microsoft.AspNetCore.Mvc;
using MovieGallery.Models;
using MovieGallery.Services.Interfaces;
using MovieGallery.Agents;

namespace MovieGallery.Controllers
{

    public class MoviesController : Controller
    {
        List<string> genreList = new List<string>
            {
                "Action",
                "Animation",
                "Biography",
                "Documentary",
                "Drama",
                "Fantasy",
                "Horror",
                "Comedy",
                "Crime",
                "Musical",
                "Adventure",
                "Romance",
                "Sci-Fi",
                "Thriller",
                "Western",
                "Superhero",
                "History"
            };
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;
        private readonly MovieGalleryHelper _chatClient;
        public MoviesController(IMovieService movieService, IReviewService reviewService, MovieGalleryHelper chatClient)
        {
            _movieService = movieService;
            _reviewService = reviewService;
            _chatClient = chatClient;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _movieService.GetTitle());
        }
        public async Task<IActionResult> GetPoster(int id)
        {
            var poster = await _movieService.Poster(id);
            if (poster == null)
            {
                return File("~/Img/Placeholder.jpg", "image/jpeg");
            }
            return File(poster, "image/jpeg");
        }

        [HttpGet]
        public IActionResult Add()
        {
            genreList.Sort();
            ViewBag.Genre = genreList;

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Movie movie, string[] Genre)
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file != null && file.Length > 0)
            {
                if (file.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("Poster", "Only JGP/JPEG format is allowed");
                    return View(movie);
                }

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg")
                {
                    ModelState.AddModelError("Poster", "File must have .JPG or .JPEG format");
                    return View(movie);
                }
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    movie.Poster = ms.ToArray();
                }
            }

            movie.Genre = string.Join(", ", Genre);

            await _movieService.Add(movie);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.Details(id);
            if (movie != null)
            {
                var reviews = await _reviewService.GetReviews(id);

                var viewModel = new MovieDetailsViewModel
                {
                    Movie = movie,
                    Reviews = reviews
                };

                return View(viewModel);
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.Get(id);
            if (movie != null) {
                if (!string.IsNullOrEmpty(movie.Genre))
                    movie.SelectedGenres = movie.Genre.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList();

                genreList.Sort();
                ViewBag.Genre = genreList;

                return View(movie);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Movie movie)
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file != null && file.Length > 0)
            {
                if (file.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("Poster", "Only JGP/JPEG format is allowed");
                    return View(movie);
                }

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg")
                {
                    ModelState.AddModelError("Poster", "File must have .JPG or .JPEG format");
                    return View(movie);
                }
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    movie.Poster = ms.ToArray();
                }
            }

            movie.Genre = string.Join(", ", movie.SelectedGenres);

            await _movieService.Update(movie);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Review(int id)
        {
            var model = new Review { MovieId = id};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(Review review)
        {
            review.ReviewerName = User.Identity.Name.ToString();
            
            await _reviewService.Add(review);
            return RedirectToAction("Details", new { id = review.MovieId });
        }

        [HttpPost]
        public async Task<IActionResult> AskHelper([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return BadRequest(new { error = "You can't send empty message" });

            var answer = await _chatClient.AskAsync(request.Query);

            return Json(new { answer });
        }
        public record ChatRequest(string Query);
    }
}