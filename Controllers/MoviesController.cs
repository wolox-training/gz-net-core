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

        public IActionResult Index()
        {
            return View(movieRepository.GetAll());
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

        public MovieRepository movieRepository
        {            
            get { return _movieRepository; }        
        }
    }
}
