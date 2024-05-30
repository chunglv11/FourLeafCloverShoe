using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;

namespace FourLeafCloverShoe.Services
{
    public class RateService : IRateService
    {
        private readonly MyDbContext _myDbContext;

        public RateService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Rate obj)
        {
            try
            {
                await _myDbContext.Rates.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Rate> lstobj)
        {
            try
            {
                await _myDbContext.Rates.AddRangeAsync(lstobj);
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
                    _myDbContext.Rates.Remove(obj);
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

        public async Task<Rate> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Rates.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Rate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Rate();
            }
        }

        public async Task<List<Rate>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Rates.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Rate>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Rate>();
            }
        }
        public async Task<bool> UpdateDanhGia(Guid id, Guid idhdct, float soSao, string? binhLuan, string? imageUrl)
        {
            try
            {
                var objFromDb = await GetById(id);
                if (objFromDb != null)
                {
                    objFromDb.Contents = binhLuan;
                    objFromDb.Reply = null;
                    objFromDb.ImageUrl = imageUrl;
                    objFromDb.Rating = soSao;
                    objFromDb.Status =  1;
                    objFromDb.CreateDate = DateTime.Now;
                    objFromDb.OrderItemId = idhdct;
                    _myDbContext.Rates.Update(objFromDb);
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Update(Rate obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Contents = obj.Contents;
                    objFromDb.Reply = obj.Reply;
                    objFromDb.ImageUrl = obj.ImageUrl;
                    objFromDb.Rating = obj.Rating;
                    objFromDb.Status = obj.Status;
                    _myDbContext.Rates.Update(objFromDb);
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

        public async Task<List<RateViewModel>> GetByIdProduct(Guid IdPr)
        {
            var query = await(from sp in _myDbContext.Products.Where(p => p.Id == IdPr)
                              join ctsp in _myDbContext.ProductDetails on sp.Id equals ctsp.ProductId
                              join cthd in _myDbContext.OrderItems on ctsp.Id equals cthd.ProductDetailId
                              join dg in _myDbContext.Rates.Where(p => p.Status == 1) on cthd.Id equals dg.OrderItemId
                              join hd in _myDbContext.Orders on cthd.OrderId equals hd.Id
                              join kc in _myDbContext.Sizes on ctsp.SizeId equals kc.Id
                              select new RateViewModel()
                              {
                                  ID = dg.Id,
                                  Rating = dg.Rating,
                                  Contents = dg.Contents,
                                  Status = dg.Status,
                                  //TenKH = _myDbContext.Users.FirstOrDefault(p => p.Id == hd.UserId).FullName,
                                  Size = kc.Name,
                                  
                              }).ToListAsync();
            return query;
        }
    }
}