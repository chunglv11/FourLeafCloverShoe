using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class Size
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public virtual List<ProductDetail>? ProductDetails { get; set; }
    }
}
