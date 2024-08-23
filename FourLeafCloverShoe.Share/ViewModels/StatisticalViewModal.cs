using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class StatisticalViewModal
    {
        public decimal Doanhthu { get; set; } // thống kê doanh thu trong hôm nay
        public int DonHangMoi { get; set; }// số lượng đơn hàng mới trong hôm nay
        public int DonHuy { get; set; }//số lượng đơn hủy mới trong hôm nay
        public List<DoanhThuViewModel> DoanhThuTrong7ngay { get; set; } // thống kê doanh thu trong 7 ngày
        public List<TrangThaiDonHangViewModel> TrangThai { get; set; } //thông kê trạng thái các đơn hàng trong hôm nay 
        public List<TopSanPhamViewModel> Topsp { get; set; } // thống kê top sản phẩm trong hôm nay         
    }
}
