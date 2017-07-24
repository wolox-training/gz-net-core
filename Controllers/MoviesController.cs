using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MvcMovie.Models.Database;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

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
            /*List<Movie> movies = null;
            IQueryable<string> genreQuery;*/
            var movieGenreVM = movieRepository.GetMovieGenreVM(searchString, movieGenre);

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
        
        public MovieRepository movieRepository
        {            
            get { return _movieRepository; }        
        }
    }
}
