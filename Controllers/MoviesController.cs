using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieGallery.Agents;
using MovieGallery.Models;
using MovieGallery.Services.Interfaces;
using System.Security.Cryptography;

namespace MovieGallery.Controllers
{

    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;
        private readonly MovieGalleryHelper _chatClient;
        public MoviesController(IMovieService movieService, IReviewService reviewService, IUserService userService, MovieGalleryHelper chatClient)
        {
            _movieService = movieService;
            _reviewService = reviewService;
            _userService = userService;
            _chatClient = chatClient;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _movieService.GetTitle());
        }
        public async Task<IActionResult> GetPoster(int id)
        {
            var poster = await _movieService.Poster(id);
            Response.Headers["Cache-Control"] = "no-cache, must-revalidate";

            if (poster is null)
            {
                return File("~/Img/Placeholder.jpg", "image/jpeg");
            }

            var etag = $"\"{Convert.ToHexString(SHA256.HashData(poster))}\"";
            Response.Headers["ETag"] = etag;

            var requestEtags = Request.Headers["If-None-Match"]
                .ToString()
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (requestEtags.Contains(etag, StringComparer.Ordinal) ||
                requestEtags.Contains("*", StringComparer.Ordinal))
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            return File(poster, "image/jpeg");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Genre = Movie.Genres;

            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.Get(id);
            if (movie != null) {
                if (!string.IsNullOrEmpty(movie.Genre))
                    movie.SelectedGenres = movie.Genre.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList();

                ViewBag.Genre = Movie.Genres;

                return View(movie);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
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

            var answer = await _chatClient.AskAsync(request.Query, request.History);

            return Json(new { answer });
        }

        public record ChatRequest(string Query, List<ChatTurn>? History);

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AdminPanel()
        {
            return View(await _userService.GetAll());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(UserData userData)
        {
            var currentUserId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userData.Id == currentUserId) return RedirectToAction(nameof(AdminPanel));

            await _userService.UpdateRole(userData);
            return RedirectToAction(nameof(AdminPanel));
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}