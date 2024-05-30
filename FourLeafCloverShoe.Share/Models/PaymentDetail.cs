using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class PaymentDetail
    {
        public Guid Id { get; set; }
        public Guid IdPayment { get; set; }
        public Guid IdOrder { get; set; }
        public decimal? TotalMoney { get; set; }//tiền mặt hoặc tiền trả qr 
        public string? Note { get; set; }
        public int Status { get; set; }
        [ForeignKey("IdPayment")]
        public virtual PaymentType PaymentType { get; set; }
        [ForeignKey("IdOrder")]
        public virtual Order Order { get; set; }
    }
}
