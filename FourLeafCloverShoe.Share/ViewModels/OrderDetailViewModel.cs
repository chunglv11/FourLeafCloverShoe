using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class OrderDetailViewModel
    {
        public Guid ID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? OrderCode { get; set; } // mã bill
        public decimal? TotalAmout { get; set; } // tổng tiền 
        public DateTime? Ship_Date { get; set; } // ngày Ship
        public DateTime? Delivery_Date { get; set; } // ngày nhận hàng 
        public string? Description { get; set; } // mô tả
        public string? RecipientName { get; set; }
        public string? RecipientPhone { get; set; }
        public string? RecipientAddress { get; set; }
        public decimal? ShippingFee { get; set; }
        public string? PaymentType { get; set; }
        public int? OrderStatus { get; set; }
        public Guid IDOrderIterm { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }
        public int? Quantity { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public string? FullName { get; set; }
        public decimal? CoinUsed { get; set; }
        public decimal? Coin { get; set; }
        public int? StatusRate { get; set; }
        public decimal? VoucherValue { get; set; }
        public int? VoucherType { get; set; }
        public string? SKU { get; set; }
    }
}
