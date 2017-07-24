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

        public Movie GetById(int? id)
        {
            using(var context = Context)
            {
                return context.Set<Movie>().Find(id);
            }
        }

        public List<Movie> GetAll()
        {
            using(var context = Context)
            {
                return context.Set<Movie>().ToList();
            }
        }

        public void Update(Movie entity)
        {
            using(var context = Context)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                context.Set<Movie>().Update(entity);
                context.SaveChanges();
            }
        }

        public void Insert(Movie entity)
        {
            using(var context = Context)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                context.Set<Movie>().Add(entity);
                context.SaveChanges();
            }
        }

        public void Delete(int? id)
        {
            using(var context = Context)
            {
                Movie movie = context.Set<Movie>().Find(id);
                if (movie == null)
                {
                    throw new ArgumentNullException();
                }
                
                try
                {
                    context.Set<Movie>().Remove(movie);
                    context.SaveChanges();
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
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
