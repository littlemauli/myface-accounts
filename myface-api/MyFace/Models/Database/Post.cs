using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace MyFace.Models.Database
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PostedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
    }
}