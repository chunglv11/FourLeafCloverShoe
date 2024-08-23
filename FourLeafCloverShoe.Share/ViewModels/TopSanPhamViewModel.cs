using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class TopSanPhamViewModel
    {
        public Guid id { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuongSanPhamBanRaTrongTuan { get; set; }
        public string MaSanPham { get; set; } 
    }
}
