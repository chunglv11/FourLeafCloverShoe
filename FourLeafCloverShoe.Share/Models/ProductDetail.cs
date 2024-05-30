using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class ProductDetail
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid ColorId { get; set; }
        public string? SKU { get; set; }
        public int? Quantity { get; set; }
        public decimal? PriceSale { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? Status { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Products { get; set; }

        [ForeignKey("SizeId")]
        public virtual Size? Size { get; set; }
        [ForeignKey("MaterialId")]
        public virtual Material? Material { get; set; }
        [ForeignKey("ColorId")]
        public virtual Colors? Colors { get; set; }
        public virtual List<CartItem>? CartItems { get; set; }
        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}
