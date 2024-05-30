using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly MyDbContext _myDbContext;

        public ProductImageService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(ProductImages obj)
        {
            try
            {
                await _myDbContext.ProductImages.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<ProductImages> lstobj)
        {
            try
            {
                await _myDbContext.ProductImages.AddRangeAsync(lstobj);
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
                    _myDbContext.ProductImages.Remove(obj);
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

        public async Task<ProductImages> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.ProductImages.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new ProductImages();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new ProductImages();
            }
        }

        public async Task<List<ProductImages>> Gets()
        {
            try
            {
                var obj = await _myDbContext.ProductImages.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<ProductImages>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<ProductImages>();
            }
        }

        public async Task<bool> Update(ProductImages obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                    _myDbContext.ProductImages.Update(objFromDb);
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
