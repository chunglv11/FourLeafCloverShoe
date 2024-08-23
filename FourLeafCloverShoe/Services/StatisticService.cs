using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly MyDbContext _myDbContext;

        public StatisticService(MyDbContext context)
        {
            _myDbContext = context;
        }
        //public async Task<StatisticalViewModal> GetStatistics(int? month, int? year)
        //{
        //    var query = _myDbContext.Orders.ToList();


        //    if (month != 0)
        //    {
        //        query = query.Where(o => o.PaymentDate.Value.Month == month.Value && o.PaymentDate.HasValue).ToList();
        //    }

        //    if (year != 0)
        //    {
        //        query = query.Where(o => o.PaymentDate.Value.Year == year.Value && o.PaymentDate.HasValue).ToList();
        //    }

        //    var today = DateTime.Now.Date;
        //    var sevenDaysAgo = today.AddDays(-6);

        //    // Doanh thu trong ngày hôm nay
        //    var doanhThuTrongNgay = await _myDbContext.Orders
        //        .Where(o => o.OrderStatus == 8 && o.PaymentDate.Value.Date == today) // Giả sử trạng thái 8 có nghĩa là đơn hàng đã hoàn thành
        //        .SelectMany(o => o.OrderItems) // Mở rộng ra các mặt hàng trong đơn hàng
        //        .GroupBy(oi => oi.Orders.PaymentDate.Value.Date)
        //        .Select(g => new DoanhThuViewModel
        //        {
        //            NgayBan = g.Key,
        //            DoanhThu = g.Sum(oi => (oi.Quantity ?? 0) * (oi.Price ?? 0))
        //        })
        //        .FirstOrDefaultAsync() ?? new DoanhThuViewModel { NgayBan = today, DoanhThu = 0 };

        //    var doanhThuquerys = query
        //     .Where(o => o.OrderStatus == 8 && o.PaymentDate >= sevenDaysAgo) /// Lọc theo trạng thái đơn và ngày
        //      .SelectMany(o => o.OrderItems) // Lấy tất cả OrderItems liên quan
        //      //.GroupBy(oi => oi.Orders.PaymentDate.Value.Date) // Nhóm theo ngày của đơn hàng
        //      //.Select(g => new DoanhThuViewModel
        //      //{
        //      //    NgayBan = g.Key,
        //      //    DoanhThu = g.Sum(oi => (oi.Quantity ?? 0) * (oi.Price ?? 0)) // Tính tổng doanh thu
        //      //})
        //       .ToList();

        //    // Doanh thu trong 7 ngày qua
        //    var doanhThuquery = query
        //        .Where(o => o.OrderStatus == 8 && o.PaymentDate >= sevenDaysAgo) // Lọc theo trạng thái đơn và ngày
        //        .SelectMany(o => o.OrderItems) // Lấy tất cả OrderItems liên quan
        //        .GroupBy(oi => oi.Orders.PaymentDate.Value.Date) // Nhóm theo ngày của đơn hàng
        //        .Select(g => new DoanhThuViewModel
        //             {
        //                 NgayBan = g.Key,
        //                 DoanhThu = g.Sum(oi => (oi.Quantity ?? 0) * (oi.Price ?? 0)) // Tính tổng doanh thu
        //             })
        //         .ToList();

        //    // Số lượng đơn hàng mới trong ngày hôm nay
        //    var tomorrow = today.AddDays(1);

        //    var donHangMoi = await _myDbContext.Orders
        //        .CountAsync(o => o.CreateDate >= today && o.CreateDate < tomorrow);


        //    // Số lượng đơn hàng bị hủy trong ngày hôm nay
        //    var donHuy = await _myDbContext.Orders
        //        .CountAsync(o => o.CreateDate >= today && o.CreateDate < tomorrow && o.OrderStatus == 13);

        //    // Top sản phẩm bán chạy trong ngày hôm na


        //    // Trạng thái đơn hàng trong ngày hôm nay (cần phải được định nghĩa trước)
        //    var trangThai = await _myDbContext.Orders
        //        .Where(o => o.CreateDate >= today)
        //        .GroupBy(o => o.OrderStatus)
        //        .Select(g => new TrangThaiDonHangViewModel
        //        {
        //            trangthai = (int)g.Key,
        //            soluong = g.Count()
        //        })
        //        .ToListAsync();

        //    return new StatisticalViewModal
        //    {
        //        Doanhthu = doanhThuTrongNgay.DoanhThu,
        //        DonHangMoi = donHangMoi,
        //        DonHuy = donHuy,
        //        DoanhThuTrong7ngay = doanhThuquery,
        //        TrangThai = trangThai,
        //        Topsp = GetTopRevenueProducts()
        //    };
        //}

        public async Task<StatisticalViewModal> GetStatistics(int? month, int? year)
        {
            var today = DateTime.Now.Date;
            var sevenDaysAgo = today.AddDays(-6);

            // Lấy tất cả các đơn hàng
            var query = _myDbContext.Orders.AsQueryable();

            // Lọc theo tháng và năm nếu có giá trị
            if (month.HasValue)
            {
                query = query.Where(o => o.PaymentDate.HasValue && o.PaymentDate.Value.Month == month.Value);
            }

            if (year.HasValue)
            {
                query = query.Where(o => o.PaymentDate.HasValue && o.PaymentDate.Value.Year == year.Value);
            }

            // Doanh thu trong ngày hôm nay
            var doanhThuTrongNgay = await _myDbContext.Orders
                .Where(o => o.OrderStatus == 8 && o.PaymentDate.HasValue && o.PaymentDate.Value.Date == today) // Giả sử trạng thái 8 có nghĩa là đơn hàng đã hoàn thành
                .SelectMany(o => o.OrderItems) // Mở rộng ra các mặt hàng trong đơn hàng
                .GroupBy(oi => oi.Orders.PaymentDate.Value.Date)
                .Select(g => new DoanhThuViewModel
                {
                    NgayBan = g.Key,
                    DoanhThu = g.Sum(oi => (oi.Quantity ?? 0) * (oi.Price ?? 0))
                })
                .FirstOrDefaultAsync() ?? new DoanhThuViewModel { NgayBan = today, DoanhThu = 0 };

            // Doanh thu trong 7 ngày qua
            var doanhThuquery = await query
                .Where(o => o.OrderStatus == 8 && o.PaymentDate >= sevenDaysAgo) // Lọc theo trạng thái đơn và ngày
                .SelectMany(o => o.OrderItems) // Lấy tất cả OrderItems liên quan
                .GroupBy(oi => oi.Orders.PaymentDate.Value.Date) // Nhóm theo ngày của đơn hàng
                .Select(g => new DoanhThuViewModel
                {
                    NgayBan = g.Key,
                    DoanhThu = g.Sum(oi => (oi.Quantity ?? 0) * (oi.Price ?? 0)) // Tính tổng doanh thu
                })
                .ToListAsync();

            // Số lượng đơn hàng mới trong ngày hôm nay
            var tomorrow = today.AddDays(1);

            var donHangMoi = await _myDbContext.Orders
                .CountAsync(o => o.CreateDate.HasValue && o.CreateDate.Value >= today && o.CreateDate.Value < tomorrow);

            // Số lượng đơn hàng bị hủy trong ngày hôm nay
            var donHuy = await _myDbContext.Orders
                .CountAsync(o => o.CreateDate.HasValue && o.CreateDate.Value >= today && o.CreateDate.Value < tomorrow && o.OrderStatus == 13);

            // Trạng thái đơn hàng trong ngày hôm nay
            var trangThai = await _myDbContext.Orders
                .Where(o => o.CreateDate.HasValue && o.CreateDate.Value.Date == today)
                .GroupBy(o => o.OrderStatus)
                .Select(g => new TrangThaiDonHangViewModel
                {
                    trangthai = (int)g.Key,
                    soluong = g.Count()
                })
                .ToListAsync();

            // Top sản phẩm bán chạy
            var topSp = GetTopRevenueProducts();

            return new StatisticalViewModal
            {
                Doanhthu = doanhThuTrongNgay.DoanhThu,
                DonHangMoi = donHangMoi,
                DonHuy = donHuy,
                DoanhThuTrong7ngay = doanhThuquery,
                TrangThai = trangThai,
                Topsp = topSp
            };
        }

        public List<TopSanPhamViewModel> GetTopRevenueProducts()
        {

            var salesData = _myDbContext.OrderItems
                .Where(oi => oi.Orders.OrderStatus == 8)
                .GroupBy(oi => oi.ProductDetailId)
                .Select(g => new
                {
                    ProductDetailId = g.Key,
                    TotalQuantitySold = g.Sum(oi => oi.Quantity ?? 0)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(7)
                .ToList();
            var productIds = _myDbContext.ProductDetails
               .Where(pd => salesData.Select(sd => sd.ProductDetailId).Contains(pd.Id))
               .Select(pd => pd.ProductId)
               .Distinct()
               .ToList();
            var products = _myDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            var topProducts = products
      .Select(p => new TopSanPhamViewModel
      {
          id = p.Id,
          MaSanPham = p.ProductName,
          TenSanPham = p.ProductCode,
          SoLuongSanPhamBanRaTrongTuan = salesData
                .FirstOrDefault(sd => _myDbContext.ProductDetails
                    .Where(pd => pd.Id == sd.ProductDetailId)
                    .Select(pd => pd.ProductId)
                    .FirstOrDefault() == p.Id)?.TotalQuantitySold ?? 0
      }).
      OrderByDescending(c=>c.SoLuongSanPhamBanRaTrongTuan)
      .ToList();

            return topProducts;

        }
    }
}
