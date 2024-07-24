using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
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
        public string? FullName { get; set; }
        public string? NameStaff { get; set; }
        public int? OrderStatus { get; set; }
    }
}
