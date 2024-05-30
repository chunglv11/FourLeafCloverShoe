using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public int? AvailableQuantity { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Categories { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand? Brands { get; set; }

        public virtual List<ProductDetail>? ProductDetails { get; set; }
        public virtual List<ProductImages>? ProductImages { get; set; }
    }
}
