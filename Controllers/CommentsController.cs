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

namespace MvcMovie.Controllers
{
    public class CommentsController : Controller
    {
        private readonly DbContextOptions<DataBaseContext> _options;
        private readonly CommentRepository _commentRepository;

        public CommentsController(DbContextOptions<DataBaseContext> options)
        {
            this._options = options;
            this._commentRepository = new CommentRepository(_options);
        }

        public IActionResult Create(int MovieID, string Content)
        {
            Comment comment = new Comment();
            comment.MovieID = MovieID;
            comment.Content = Content;
            commentRepository.Insert(comment);
            return RedirectToAction("Details", "Movies", new {@id=MovieID});
        }

        public CommentRepository commentRepository
        {            
            get { return _commentRepository; }        
        }
    }
}
