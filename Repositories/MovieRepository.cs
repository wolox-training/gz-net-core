using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models.Database;

namespace MvcMovie.Repositories
{
    public class MovieRepository
    {
        private readonly DbContextOptions<DataBaseContext> _options;

        public MovieRepository(DbContextOptions<DataBaseContext> options)
        {
            this._options = options;
        }

        public List<Movie> GetAll()
        {
            using(var context = Context)
            {
                return context.Set<Movie>().ToList();
            }
        }

        public DbContextOptions<DataBaseContext> Options
        {
            get { return _options; }
        }

        public DataBaseContext Context
        {
            get { return new DataBaseContext(Options); }
        }
    }    
}
