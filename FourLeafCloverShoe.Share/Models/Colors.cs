using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class Colors
    {
        public Guid Id { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
        public virtual List<ProductDetail>? ProductDetails { get; set; }
    }
}
