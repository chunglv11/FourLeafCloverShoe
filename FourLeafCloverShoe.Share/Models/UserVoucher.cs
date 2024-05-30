using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class UserVoucher
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public Guid? VoucherId { get; set; }
        public int? Status { get; set; }  // chua su dung =1, da su dung =-1
        [ForeignKey("UserId")]
        public virtual User? Users { get; set; }
        [ForeignKey("VoucherId")]
        public virtual Voucher? Vouchers { get; set; }
    }
}
