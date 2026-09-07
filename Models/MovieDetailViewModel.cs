using System.Collections.Generic;

namespace MovieGallery.Models
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}