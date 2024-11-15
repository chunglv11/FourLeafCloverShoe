﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class VoucherViewModel
    {
        public Guid? Id { get; set; }
        public string? VoucherCode { get; set; } //
        public string? Title { get; set; } //
        public int? Quantity { get; set; } // số lượng
        public decimal? VoucherValue { get; set; } // giá trị voucher
        public int? VoucherType { get; set; } // hinh thuc giam gia, 1 là %, 0 là theo tiền mặt
        public int? MinimumOrderValue { get; set; } // giá trị đơn hàng tối thiểu ( ví dụ mã áp dụng cho đơn từ 50k )
        public int? MaximumOrderValue { get; set; } // số tiền giảm tối đa
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<string> Ranks { get; set; }
        public int? Status { get; set; } // da kich hoat or No
    }
}
