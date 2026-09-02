using Microsoft.AspNetCore.Mvc;
using MovieGallery.Models;
using MovieGallery.Services.Interfaces;

namespace MovieGallery.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
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
            var genreList = new List<string>
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
                "Superhero"
            };

            genreList.Sort();
            ViewBag.Genres = genreList;

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Movie movie, string[] Genres)
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

            movie.Genre = string.Join(", ", Genres);

            await _movieService.Add(movie);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.Details(id);
            if (movie != null)
                return View(movie);

            return RedirectToAction("Index");
        }

    }
}
