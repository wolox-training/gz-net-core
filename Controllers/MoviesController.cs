using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
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

        public MovieRepository movieRepository
        {            
            get { return _movieRepository; }        
        }
    }
}
