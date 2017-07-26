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

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DbContextOptions<DataBaseContext> _options;
        private readonly MovieRepository _movieRepository;

        public MoviesController(DbContextOptions<DataBaseContext> options)
        {
            this._options = options;
            this._movieRepository = new MovieRepository(_options);
        }

        public IActionResult Index(string searchString, string movieGenre, string sortOrder) 
        {
            var movieGenreVM = new MovieViewModel();
            ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title"; 
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["GenreSortParm"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
            
            List<Movie> movies = movieRepository.GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre).ToList();
            }
            movieGenreVM.genres = new List<SelectListItem>();

            movies = SortMovieList(sortOrder, movies);

            foreach (string genreStr in movieRepository.GetAllGenre())
            {
                movieGenreVM.genres.Add(new SelectListItem { Text = genreStr, Value = genreStr });
            }
            movieGenreVM.movies = movies;
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

        private List<Movie> SortMovieList(string sortOrder, List<Movie> movies)
        {
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
