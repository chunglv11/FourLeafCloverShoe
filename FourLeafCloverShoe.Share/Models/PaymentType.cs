using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class PaymentType
    {
        //[Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public virtual List<PaymentDetail>? Orders { get; set; }
    }
}
