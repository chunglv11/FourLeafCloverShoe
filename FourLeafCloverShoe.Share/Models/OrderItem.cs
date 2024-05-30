using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ProductDetailId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        [ForeignKey("OrderId")]
        public Order? Orders { get; set; }
        [ForeignKey("ProductDetailId")]
        public ProductDetail? ProductDetails { get; set; }
        // Thêm thuộc tính navigation để thiết lập quan hệ 1-1
        public virtual Rate? Rate { get; set; }
    }
}
