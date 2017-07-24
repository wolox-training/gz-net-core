using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models.Database
{
    public class Comment
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int MovieID { get; set; }

        [Required]
        public Movie Movie { get; set; }
    }
}
