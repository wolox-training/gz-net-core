using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models.Views;
using MvcMovie.Models.Database;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcMovie.Repositories
{
    public class CommentRepository
    {
        private readonly DbContextOptions<DataBaseContext> _options;

        public CommentRepository(DbContextOptions<DataBaseContext> options)
        {
            this._options = options;
        }

        public void Insert(Comment entity)
        {
            using(var context = Context)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                context.Set<Comment>().Add(entity);
                context.SaveChanges();
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
