using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using MvcMovie.Models.Database;

namespace MvcMovie.Models.Views
{
    public class MovieViewModel
    {
        public List<Movie> movies;
        public List<SelectListItem> genres;
        public string movieGenre { get; set; }
    }
}

