using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid? CartId { get; set; }
        public Guid? ProductDetailId { get; set; }
        public int? Quantity { get; set; }
        [ForeignKey("CartId")]
        public Cart? Cart { get; set; }
        [ForeignKey("ProductDetailId")]
        public ProductDetail? ProductDetails { get; set; }
    }
}
