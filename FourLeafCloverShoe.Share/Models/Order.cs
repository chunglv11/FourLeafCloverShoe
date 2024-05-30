using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }//
        public string? StaffId { get; set; } // id nhan vien
        public Guid? VoucherId { get; set; } //
        public string? OrderCode { get; set; }//
        public string? PaymentType { get; set; }//
        public int? OrderStatus { get; set; }//
        // -1 --> đơn chờ
        // 0 --> chờ thanh toán
        // 1 --> đã thanh toán                                       ----> (user)chờ xác nhận  --> admin duyệt ( nhấn nút : xác nhận đơn)
        // 2 --> chờ xác nhận
        //0-2-3 sửa dc

        // 3 --> chờ lấy hàng                                       ----> (user)chờ lấy hàng   --> admin duyệt ( nhấn nút : giao hàng)


        // 4 --> đang giao hàng                                      ----> (user)đang giao   --> admin duyệt yêu cầu huỷ
        // 5  --> khách yêu cầu huỷ (trong khi )
        // 6--> giao hàng thất bại   
        // 7 --> yêu cầu huỷ đơn thất bại

        // 8--> giao hàng thành công                                 --> admin duyệt trạng thái
        // 9-> thanh toán tại quầy
        // 10 -->yêu cầu đổi trả
        // 11 --> chấp nhận đổi trả
        // 12 --> từ chối đổi trả

        // 13 --> khách huỷ đơn                                          ---- > đã huỷ
        // 14 --> chấp nhận huỷ đơn



        public string? RecipientName { get; set; }
        public string? RecipientAddress { get; set; }// địa chỉ
        public string? RecipientPhone { get; set; }// sdt
        public decimal? CoinsUsed { get; set; } // tổng tiền trước khi áp dụng
        public decimal? TotalAmout { get; set; } // tổng tiền trước khi áp dụng
        public decimal? VoucherValue { get; set; } // giá trị voucher
        public decimal? ShippingFee { get; set; } // phí ship
        public DateTime? CreateDate { get; set; } // ngày tạo bill
        public DateTime? UpdateDate { get; set; } // ngày update bill
        public DateTime? ShipDate { get; set; } // ngày Ship
        public DateTime? PaymentDate { get; set; } // ngày thanh toán
        public DateTime? DeliveryDate { get; set; } // ngày nhận hàng
        public string? Description { get; set; } // mo ta
        [ForeignKey("UserId")]
        public virtual User? Users { get; set; }
        [ForeignKey("StaffId")]
        [NotMapped]
        public virtual User? Staff { get; set; }
        [ForeignKey("VoucherId")]

        public virtual Voucher? Vouchers { get; set; }

        public virtual List<OrderItem>? OrderItems { get; set; }
        public virtual List<PaymentDetail>? PaymentDetails { get; set; }
    }
}
