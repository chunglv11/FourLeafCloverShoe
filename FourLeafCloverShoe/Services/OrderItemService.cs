using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FourLeafCloverShoe.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly MyDbContext _myDbContext;

        public OrderItemService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(OrderItem obj)
        {
            try
            {
                await _myDbContext.OrderItems.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<OrderItem> lstobj)
        {
            try
            {
                await _myDbContext.OrderItems.AddRangeAsync(lstobj);
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
                    _myDbContext.OrderItems.Remove(obj);
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

        public async Task<bool> DeleteMany(List<OrderItem> lstobj)
        {
            try
            {
                 _myDbContext.OrderItems.RemoveRange(lstobj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<OrderItem> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.OrderItems.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new OrderItem();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new OrderItem();
            }
        }
        public async Task<List<OrderDetailViewModel>> GetByIdOrder(Guid IdOrder)
        {
            var get = await _myDbContext.OrderItems.FindAsync(IdOrder);
            var lstOrderDetail = await (from a in _myDbContext.OrderItems
                                        where a.OrderId == IdOrder
                                        join b in _myDbContext.Orders on a.OrderId equals b.Id
                                        join d in _myDbContext.ProductDetails on a.ProductDetailId equals d.Id
                                        join e in _myDbContext.Sizes on d.SizeId equals e.Id
                                        join f in _myDbContext.Colors on d.ColorId equals f.Id
                                        join g in _myDbContext.Products on d.ProductId equals g.Id
                                        join x in _myDbContext.Users on b.UserId equals x.Id
                                        join c in _myDbContext.Rates on a.Id equals c.Id into rateJoin
                                        from c in rateJoin.DefaultIfEmpty()
                                        select new OrderDetailViewModel()
                                        {
                                            ID = b.Id,
                                            ProductDetailID = d.Id,
                                            CoinUsed = b.CoinsUsed,
                                            Coin = x.Coins,
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
                                            IDOrderIterm = a.Id,
                                            Price = a.Price,
                                            Quantity = a.Quantity,
                                            SKU = d.SKU,
                                            PriceSale = d.PriceSale,
                                            SizeName = e.Name,
                                            ColorName = f.ColorName,
                                            ProductName = g.ProductName,
                                            ImageUrl = _myDbContext.ProductImages.First(c => c.ProductId == g.Id).ImageUrl,
                                            VoucherType = b.VoucherId == null ? null : (_myDbContext.Vouchers.FirstOrDefault(c => c.Id == b.VoucherId)).VoucherType,
                                            VoucherValue = b.VoucherId == null ? 0 : (_myDbContext.Vouchers.FirstOrDefault(c => c.Id == b.VoucherId)).VoucherValue,
                                            FullName = b.UserId == null ? null : (_myDbContext.Users.FirstOrDefault(c => c.Id == b.UserId)).FullName,
                                            StatusRate = c != null ? c.Status : 0

                                        }).ToListAsync();
            return lstOrderDetail;
        }
        public async Task<OrderDetailViewModel1> GetByIdOrder1(Guid? IdOrder)
        {
            var get = await _myDbContext.OrderItems.FindAsync(IdOrder);
            var lstOrderDetail = await (from a in _myDbContext.OrderItems
                                        where a.OrderId == IdOrder
                                        join b in _myDbContext.Orders on a.OrderId equals b.Id
                                        join d in _myDbContext.ProductDetails on a.ProductDetailId equals d.Id
                                        join e in _myDbContext.Sizes on d.SizeId equals e.Id
                                        join f in _myDbContext.Colors on d.ColorId equals f.Id
                                        join g in _myDbContext.Products on d.ProductId equals g.Id
                                        join x in _myDbContext.Users on b.UserId equals x.Id
                                        join c in _myDbContext.Rates on a.Id equals c.Id into rateJoin
                                        from c in rateJoin.DefaultIfEmpty()
                                        select new OrderDetailViewModel1()
                                        {
                                            Id = b.Id,
                                            CoinsUsed = b.CoinsUsed,
                                            Coin = x.Coins,
                                            CreateDate = b.CreateDate,
                                            PaymentDate = b.PaymentDate,
                                            ShipDate = b.ShipDate,
                                            DeliveryDate = b.DeliveryDate,
                                            Description = b.Description,
                                            OrderCode = b.OrderCode,
                                            TotalAmout = b.TotalAmout,
                                            RecipientName = b.RecipientName,
                                            RecipientPhone = b.RecipientPhone,
                                            RecipientAddress = b.RecipientAddress,
                                            ShippingFee = b.ShippingFee,
                                            OrderStatus = b.OrderStatus,
                                            PaymentType = b.PaymentType,



                                            OrderItem = (from hd in _myDbContext.OrderItems
                                                         join d in _myDbContext.ProductDetails on hd.ProductDetailId equals d.Id
                                                         join b in _myDbContext.Products on d.ProductId equals b.Id
                                                         join cl in _myDbContext.Colors on d.ColorId equals cl.Id
                                                         join sz in _myDbContext.Sizes on d.SizeId equals sz.Id
                                                         where hd.OrderId == IdOrder
                                                         select new OrderItemCTViewModel2
                                                         {
                                                             Id = hd.Id,
                                                             OrderId = IdOrder,
                                                             ProductDetailId = d.Id,
                                                             SKU = d.SKU,
                                                             ProductName = b.ProductName,
                                                             ProductId = b.Id,
                                                             SizeId = sz.Id,
                                                             SizeName = sz.Name,
                                                             ColorId = cl.Id,
                                                             ColorName = cl.ColorName,
                                                             Quantity = hd.Quantity ?? 0,
                                                             Price = hd.Price?? 0,
                                                             PriceSale = hd.Price ?? 0,
                                                             ProductImages = (from pi in _myDbContext.ProductImages
                                                                              where pi.ProductId == b.Id
                                                                              select pi.ImageUrl).ToList()
                                                         }).ToList()
                                        }).FirstOrDefaultAsync();
            return lstOrderDetail;
        }
        public async Task<List<OrderItem>> GetByIdOrder2(Guid IdOrder)
        {
            try
            {
                var orderItems = await _myDbContext.OrderItems
                    .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Products)
                            .ThenInclude(c => c.ProductImages)
                     .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Colors)
                   .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Size)
                    .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Size)
                    .Include(c => c.Rate)
                    .Include(c => c.Orders)
                        .ThenInclude(c=> c.Users)
                      
                        
                        
                    .Where(c => c.OrderId == IdOrder)
                    .ToListAsync();

                return orderItems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<OrderItem>();
            }
        }

        public async Task<List<OrderItem>> Gets()
        {
            try
            {
                var obj = await _myDbContext.OrderItems
                    .Include(c=>c.ProductDetails)
                        .ThenInclude(c=>c.Products)
                            .ThenInclude(c=>c.ProductImages)
                    .Include(c=>c.ProductDetails)
                        .ThenInclude(c=>c.Size)
                    .Include(c=>c.Rate)
                    
                    .ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<OrderItem>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<OrderItem>();
            }
        }

        public async Task<bool> Update(OrderItem obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Quantity = obj.Quantity;
                    objFromDb.Price = obj.Price;
                    _myDbContext.OrderItems.Update(objFromDb);
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
    }
}