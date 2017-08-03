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

namespace MvcMovie.Controllers.api.v1
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

        [HttpPost]
        public JsonResult Create(Movie movie)
        {
            object Result = new {};
            
            if (ModelState.IsValid) 
            {
                Response.StatusCode = 200;
                try
                {
                    movieRepository.Insert(movie);
                    Result = new { message = "The movie was successfully created." };
                }
                catch (Exception e)
                {
                    Response.StatusCode = 400;
                    Result = new { message = "The movie could not be created.", error = e.ToString()};
                    return Json(Result);
                }
            }
            
            else
            {
                Result = new { message = "The movie could not be created." };
            }

            return Json(Result);
        }   

        public MovieRepository movieRepository
        {            
            get { return _movieRepository; }        
        }
    }
}
