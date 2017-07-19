using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MvcMovie.Models.Database;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Repositories;

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

        public IActionResult Index(string searchString, string movieGenre) {
            List<Movie> movies = null;
            IQueryable<string> genreQuery;
            var movieGenreVM = new MovieGenreViewModel();

            using(var context = Context)
            {
                genreQuery = from m in context.Movies orderby m.Genre select m.Genre;
                movies = context.Set<Movie>().ToList();

                if (!String.IsNullOrEmpty(searchString))
                {
                    movies = movies.Where(s => s.Title.Contains(searchString)).ToList();
                }

                if (!String.IsNullOrEmpty(movieGenre))
                {
                    movies = movies.Where(x => x.Genre == movieGenre).ToList();
                }

                movieGenreVM.genres = new SelectList(genreQuery.Distinct().ToList());
                movieGenreVM.movies = movies.ToList();
            }

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

        public IActionResult Delete(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }
            movieRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }

            Movie movie = movieRepository.GetById(id);

            if (movie == null) {
                return NotFound();
            }

            return View(movie);
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
