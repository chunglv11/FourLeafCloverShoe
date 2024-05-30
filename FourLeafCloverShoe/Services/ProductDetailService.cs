using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly MyDbContext _myDbContext;

        public ProductDetailService()
        {
        }

        public ProductDetailService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(ProductDetail obj)
        {
            try
            {
                obj.CreateAt = DateTime.Now;
                await _myDbContext.ProductDetails.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<ProductDetail> lstobj)
        {
            try
            {
                await _myDbContext.ProductDetails.AddRangeAsync(lstobj);
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
                    _myDbContext.ProductDetails.Remove(obj);
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

        public async Task<ProductDetail> GetById(Guid Id)
        {
            try
            {
                var lstobj = await _myDbContext.ProductDetails
                   .Include(c => c.Products)
                        .ThenInclude(c => c.ProductImages)
                   .Include(c => c.Size)
                   .Include(c => c.Colors)

                   .ToListAsync();
                var obj = lstobj.FirstOrDefault(c => c.Id == Id);
                if (obj != null)
                {

                    return obj;
                }
                return new ProductDetail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new ProductDetail();
            }
        }

        public async Task<List<ProductDetail>> GetByProductId(Guid ProductId)
        {
            try
            {
                var lstobj = await _myDbContext.ProductDetails
                   .Include(c => c.Products)
                   .ThenInclude(c => c.ProductImages)
                   .Include(c => c.Size)
                   .Include(c => c.Colors)
                   .ToListAsync();
                var obj = lstobj.Where(c => c.ProductId == ProductId).ToList();
                if (obj != null)
                {

                    return obj;
                }
                return new List<ProductDetail>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<ProductDetail>();
            }
        }

        public async Task<List<ProductDetail>> Gets()
        {
            try
            {
                var obj = await _myDbContext.ProductDetails
                    .Include(c => c.Products)
                    .ThenInclude(c => c.ProductImages)
                    .Include(c => c.Size)
                    .ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<ProductDetail>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<ProductDetail>();
            }
        }

        public async Task<bool> Update(ProductDetail obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.SizeId = obj.SizeId;
                    objFromDb.ColorId = obj.ColorId;
                    objFromDb.SKU = obj.SKU;
                    objFromDb.Quantity = obj.Quantity;
                    objFromDb.PriceSale = obj.PriceSale;
                    objFromDb.UpdateAt = DateTime.Now;
                    objFromDb.Status = obj.Status;
                    _myDbContext.ProductDetails.Update(objFromDb);
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

        public async Task<bool> UpdateMany(List<ProductDetail> lstobj)
        {
            try
            {
                 _myDbContext.ProductDetails.UpdateRange(lstobj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}