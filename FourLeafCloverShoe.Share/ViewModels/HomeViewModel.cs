using FourLeafCloverShoe.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class HomeViewModel
    {

        public IEnumerable<Product> Products { get; set; } 
        public int TotalProducts { get; set; }
    }

}
