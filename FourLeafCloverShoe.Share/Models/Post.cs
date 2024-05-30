using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? Tittle { get; set; }
        public string? TittleImage { get; set; }
        public string? Contents { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        [ForeignKey("UserId")]
        public User? Users { get; set; }
    }
}
