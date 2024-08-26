using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class OrderService : IOrderService
    {
        private readonly MyDbContext _myDbContext;

        public OrderService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Order obj)
        {
            try
            {
                obj.CreateDate = DateTime.Now;
                await _myDbContext.Orders.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Order> lstobj)
        {
            try
            {
                await _myDbContext.Orders.AddRangeAsync(lstobj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                var obj = await GetById(Id);
                if (obj != null)
                {
                    _myDbContext.Orders.Remove(obj);
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteMany(List<Order> lstobj)
        {
            try
            {
                _myDbContext.Orders.RemoveRange(lstobj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<Order> GetById(Guid Id)
        {
            try
            {
                var obj = (await Gets()).FirstOrDefault(c => c.Id == Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Order();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Order();
            }
        }

        public async Task<List<Order>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Orders
                    .Include(c => c.OrderItems)
                        .ThenInclude(c => c.ProductDetails)
                            .ThenInclude(c => c.Products)
                            .ThenInclude(c=>c.ProductImages)
                    .Include(c => c.OrderItems)
                        .ThenInclude(c => c.ProductDetails)
                            .ThenInclude(c => c.Size)
                    .Include(c=>c.OrderItems)//them lay tu bang rate
                        .ThenInclude(c=>c.Rate)
                    .Include(c=>c.Users)
                    .ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Order>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Order>();
            }
        }

        public async Task<bool> Update(Order obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.StaffId = obj.StaffId;
                    objFromDb.VoucherId = obj.VoucherId;
                    objFromDb.OrderCode = obj.OrderCode;
                    objFromDb.CoinsUsed = obj.CoinsUsed;
                    objFromDb.PaymentType = obj.PaymentType;
                    objFromDb.OrderStatus = obj.OrderStatus;
                    objFromDb.RecipientName = obj.RecipientName;
                    objFromDb.RecipientAddress = obj.RecipientAddress;
                    objFromDb.RecipientPhone = obj.RecipientPhone;
                    objFromDb.TotalAmout = obj.TotalAmout;
                    objFromDb.VoucherValue = obj.VoucherValue;
                    objFromDb.ShippingFee = obj.ShippingFee;
                    objFromDb.UpdateDate = DateTime.Now;
                    objFromDb.ShipDate = obj.ShipDate;
                    objFromDb.PaymentDate = obj.PaymentDate;
                    objFromDb.DeliveryDate = obj.DeliveryDate;
                    objFromDb.Description = obj.Description;
                    _myDbContext.Orders.Update(objFromDb);
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateRank(int? point, decimal? coin, string? userid)
        {
            

               
                    var user =  _myDbContext.Users.FirstOrDefault(c => c.Id == userid );
                    if (user != null)
                    {
                        user.Points = point;
                        user.Coins = coin;
                        if (user.Points >= 0 && user.Points <= 1000000)
                        {
                            var ranknamne = await _myDbContext.Ranks.FirstOrDefaultAsync(c => c.Name == "Bạc");
                            user.RankId = ranknamne.Id;
                        }
                        else if (user.Points >= 2000001 && user.Points <= 30000000)
                        {
                            var ranknamne = await _myDbContext.Ranks.FirstOrDefaultAsync(c => c.Name == "Vàng");
                            user.RankId = ranknamne.Id;
                        }
                        else 
                        {
                            var ranknamne = await _myDbContext.Ranks.FirstOrDefaultAsync(c => c.Name == "Kim Cương");
                            user.RankId = ranknamne.Id;
                        }
                        _myDbContext.Users.Update(user);
                        await _myDbContext.SaveChangesAsync();
                        return true;
                    

                     }
            
            return false;
        }
        public async Task<bool> UpdateOrderStatus(Guid idOrder, int? status, string? idStaff)
        {

            var updateorder = await _myDbContext.Orders.FirstOrDefaultAsync(p => p.Id == idOrder);
            var chitiethoadon = await _myDbContext.OrderItems.Where(p => p.OrderId == idOrder).ToListAsync();

            if (updateorder != null)
            {
                if (status == 10)
                {
                    foreach (var item in chitiethoadon)
                    {
                        var CTsanPham = await _myDbContext.ProductDetails.FirstOrDefaultAsync(p => p.Id == item.ProductDetailId);
                        CTsanPham.Quantity += item.Quantity;

                        var product = await _myDbContext.Products.FindAsync(CTsanPham.ProductId);
                        if (product != null)
                        {
                            product.AvailableQuantity += item.Quantity;
                            _myDbContext.Products.Update(product);
                             await _myDbContext.SaveChangesAsync();
                        }

                        _myDbContext.ProductDetails.Update(CTsanPham);
                        await _myDbContext.SaveChangesAsync();


                    }
                }

                if (status == 8)
                {
                    var order = await _myDbContext.Orders.FirstOrDefaultAsync(p => p.Id == idOrder);

                    var kh = await _myDbContext.Users.FirstOrDefaultAsync(c => c.Id == order.UserId);
                    var hoadon = await _myDbContext.Orders.FirstOrDefaultAsync(c => c.Id == idOrder);
                    if (kh != null && hoadon != null)
                    {
                        kh.Points += Convert.ToInt32(hoadon.TotalAmout);
                        kh.Coins +=hoadon.TotalAmout / 100;
                        await  UpdateRank(kh.Points, kh.Coins, kh.Id);
                    }
                  
                    updateorder.PaymentDate ??= DateTime.Now;
                    updateorder.DeliveryDate ??= DateTime.Now;
                }

                updateorder.OrderStatus = status;
                updateorder.StaffId ??= idStaff;
                _myDbContext.Orders.Update(updateorder);
                await _myDbContext.SaveChangesAsync();


                try
                {
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    // Handle exception and possibly rollback changes
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

       

        public async Task<bool> ThanhCong(Guid idHoaDon, string? idNhanVien)
        {
            try
            {
                var hd = _myDbContext.Orders.FirstOrDefault(c => c.Id == idHoaDon);
                if (hd != null)
                {
                    hd.OrderStatus = 8;
                    hd.StaffId = idNhanVien;
                    hd.DeliveryDate = DateTime.Now;
                    hd.PaymentDate = DateTime.Now;
                    _myDbContext.Orders.Update(hd);
                    _myDbContext.SaveChanges(); // Chờ đợi lưu thay đổi vào cơ sở dữ liệu

                    // Cộng tích điểm cho khách
                    if (hd.UserId != "2FA6148D-B530-421F-878E-CE4D54BFC6AB") // nếu userid bill khác khách vãng lai thì cộng điểm
                    {
                        var kh =  _myDbContext.Users.FirstOrDefault(c => c.Id == hd.UserId);
                        if (kh != null)
                        {
                            kh.Points += Convert.ToInt32(hd.TotalAmout);
                            kh.Coins += hd.TotalAmout /100;
                            await UpdateRank(kh.Points, kh.Coins, kh.Id);
                        }
                    }

                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> HuyHD(Guid idhd, string idnv)
        {
            try
            {
                var hd = _myDbContext.Orders.Where(c => c.Id == idhd).FirstOrDefault();
                //Update hd
                hd.UserId = idnv;
                hd.OrderStatus = 13;
                hd.StaffId ??= idnv;
                //hd.TongTien = 0;
                _myDbContext.Orders.Update(hd);
                await _myDbContext.SaveChangesAsync();

                // Cộng lại số lượng hàng
                var chitiethoadon = await _myDbContext.OrderItems.Where(p => p.OrderId == idhd).ToListAsync();


                if (chitiethoadon != null)
                {


                    foreach (var item in chitiethoadon)
                    {
                        var CTsanPham = await _myDbContext.ProductDetails.FirstOrDefaultAsync(p => p.Id == item.ProductDetailId);
                        CTsanPham.Quantity += item.Quantity;

                        var product = await _myDbContext.Products.FindAsync(CTsanPham.ProductId);
                        if (product != null)
                        {
                            product.AvailableQuantity += item.Quantity;
                            _myDbContext.Products.Update(product);
                            await _myDbContext.SaveChangesAsync();

                        }

                        _myDbContext.ProductDetails.Update(CTsanPham);
                        await _myDbContext.SaveChangesAsync();

                    }

                }
                ////Cộng lại số lượng voucher nếu áp dụng
                //if (hd.VoucherId != null)
                //{
                //    var vc = await _myDbContext.Vouchers.FirstOrDefaultAsync(c => c.Id == hd.VoucherId);
                //    var uservc = await _myDbContext.VoucherUsers.FirstOrDefaultAsync(c => c.VoucherId == vc.Id && c.UserId == hd.UserId);
                //    vc.Quantity += 1;
                //    uservc.Status = true;
                //    _myDbContext.Voucher.Update(vc);
                //    _myDbContext.VoucherUser.Update(uservc);
                //    await _myDbContext.SaveChangesAsync();
                //}
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateGhiChuHD(Guid idhd, string? idnv, string ghichu)
        {
            try
            {
                var hd =  _myDbContext.Orders.FirstOrDefault(c => c.Id == idhd);
                if (ghichu == "null")
                {
                    hd.Description = null;
                    hd.UserId = idnv;
                }
                else
                {
                    hd.Description = ghichu;
                }
                _myDbContext.Orders.Update(hd);
                _myDbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<OrderViewModel>> GetOrderViewModel()
        {
            var lstOrder = await (from b in _myDbContext.Orders
                select new OrderViewModel()
                {
                    Id = b.Id,
                    CreateDate = b.CreateDate,
                    PaymentDate = b.PaymentDate,
                    Ship_Date = b.ShipDate,
                    Delivery_Date = b.DeliveryDate,
                    Description = b.Description,
                    OrderCode = b.OrderCode,
                    TotalAmout = b.TotalAmout,
                    RecipientName = b.RecipientName,
                    RecipientPhone = b.RecipientPhone,
                    RecipientAddress = b.RecipientAddress,
                    ShippingFee = b.ShippingFee,
                    OrderStatus = b.OrderStatus,
                    PaymentType = b.PaymentType,
                    FullName = b.UserId == null ? "" : (_myDbContext.Users.FirstOrDefault(c => c.Id == b.UserId)).FullName,
                    NameStaff = b.StaffId == null ? "" : (_myDbContext.Users.FirstOrDefault(c => c.Id == b.StaffId)).FullName,
                }).ToListAsync();
            return lstOrder;
        }
    }
}
