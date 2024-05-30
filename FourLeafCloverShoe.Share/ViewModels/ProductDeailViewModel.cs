using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class ProductDeailViewModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string SizeName { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Total { get; set; }
        public bool? Status { get; set; }

    }
}
