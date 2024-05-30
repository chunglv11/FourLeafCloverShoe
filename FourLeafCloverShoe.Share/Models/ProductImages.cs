using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class ProductImages
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public string? ImageUrl { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Products { get; set; }
    }
}
