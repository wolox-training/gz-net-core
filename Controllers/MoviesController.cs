using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MvcMovie.Models.Database;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovie.Models.Views;
using MvcMovie;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Globalization;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DbContextOptions<DataBaseContext> _options;
        private readonly MovieRepository _movieRepository;
        private readonly IHtmlLocalizer<MoviesController> _localizer;

        public MoviesController(DbContextOptions<DataBaseContext> options, IHtmlLocalizer<MoviesController> localizer)
        {
            this._options = options;
            this._movieRepository = new MovieRepository(_options);
            this._localizer = localizer;
        }

        public IActionResult Index(string searchString, string movieGenre, string sortOrder, string currentFilter, int? page) 
        {
            var movieGenreVM = new MovieViewModel();
            List<Movie> movies = movieRepository.GetAll();
            movies = SortMovieList(sortOrder, SearchMovies(searchString, movieGenre, movies));
            
            movieGenreVM.genres = new List<SelectListItem>();
            foreach (string genreStr in movieRepository.GetAllGenre())
            {
                movieGenreVM.genres.Add(new SelectListItem { Text = genreStr, Value = genreStr });
            }
            
            movieGenreVM.movies = PaginatedList<Movie>.Create(movies, page ?? 1, 3);
            return View(movieGenreVM);
        }

        public IActionResult Edit(int? id) 
        {
            if (id == null) 
            {
                return NotFound();
            }
            Movie movie = movieRepository.GetById(id);
            if (movie == null) 
            {
                return NotFound();
            }
            
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(int id, Movie movie) 
        {
            if (id != movie.ID) 
            {
                return NotFound();
            }
            if (ModelState.IsValid) 
            {
                movieRepository.Update(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public IActionResult New()
        {
            return View("./Views/Movies/Create.cshtml");
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid) 
            {
                movieRepository.Insert(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public IActionResult Details(int? id)
        {
            CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
            if (id == null) 
            {
                return NotFound();
            }
            Movie movie = movieRepository.GetById(id);
            if (movie == null) 
            {
                return NotFound();
            }

            ViewData["Details"] = _localizer["Details"];
            return View(movie);
        }
        
        private List<Movie> SearchMovies(string searchString, string movieGenre, List<Movie> movies)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre).ToList();
            }

            return movies;
        }
        private List<Movie> SortMovieList(string sortOrder, List<Movie> movies)
        {
            ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title"; 
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["GenreSortParm"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
            
            switch (sortOrder)
            {
                case "Title":
                    movies = movies.OrderBy(s => s.Title).ToList();
                    break;
                case "title_desc":
                    movies = movies.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Date":
                    movies = movies.OrderBy(s => s.ReleaseDate).ToList();
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.ReleaseDate).ToList();
                    break;
                case "Genre":
                    movies = movies.OrderBy(s => s.Genre).ToList();
                    break;
                case "genre_desc":
                    movies = movies.OrderByDescending(s => s.Genre).ToList();
                    break;
                default:
                    movies = movies.OrderBy(s => s.Title).ToList();
                    break;
            }

            return movies;
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }
            movieRepository.Delete(id);
            return RedirectToAction("Index");
        }
        
        public MovieRepository movieRepository
        {            
            get { return _movieRepository; }        
        }
    }
}
