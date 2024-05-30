using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class ProductService : IProductService
    {
        private readonly MyDbContext _myDbContext;

        public ProductService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Product obj)
        {
            try
            {
                obj.CreateAt = DateTime.Now;
                await _myDbContext.Products.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Product> lstobj)
        {
            try
            {
                await _myDbContext.Products.AddRangeAsync(lstobj);
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
                    _myDbContext.Products.Remove(obj);
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

        public async Task<Product> GetById(Guid Id)
        {
            try
            {

                var lstObj = await _myDbContext.Products
                   .Include(c => c.ProductDetails)
                    .Include(c => c.ProductDetails)
                       .ThenInclude(c => c.Size)
                   .Include(c => c.Categories)
                   .Include(c => c.Brands)
                   .Include(c => c.ProductImages)
                   .Include(c=>c.ProductDetails)
                   .ThenInclude(c=>c.OrderItems)
                   .ThenInclude(c=>c.Rate)
                   .ToListAsync();
                var obj = lstObj.FirstOrDefault(c => c.Id == Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Product();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Product();
            }
        }

        public async Task<List<Product>> Gets()
        {
            try
            {

                var obj = await _myDbContext.Products
                   .Include(c => c.ProductDetails)
                     .Include(c => c.ProductDetails)
                     .Include(c => c.ProductDetails)
                       .ThenInclude(c => c.Size)
                   .Include(c => c.Categories)
                   .Include(c => c.Brands)
                   .Include(c => c.ProductImages)
                    .Include(c => c.ProductDetails)
                   .ThenInclude(c => c.OrderItems)
                   .ThenInclude(c => c.Rate)
                   .ToListAsync();
                if (obj != null)
                {
                    return obj;
                }
                return new List<Product>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Product>();
            }
        }

        public async Task<bool> Update(Product obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.CategoryId = obj.CategoryId;
                    objFromDb.BrandId = obj.BrandId;
                    objFromDb.ProductCode = obj.ProductCode;
                    objFromDb.ProductName = obj.ProductName;
                    objFromDb.AvailableQuantity = obj.AvailableQuantity;
                    objFromDb.UpdateAt = DateTime.Now;
                    objFromDb.Description = obj.Description;
                    objFromDb.Status = obj.Status;
                    _myDbContext.Products.Update(objFromDb);
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

        public async Task UpdateStatusQuantity()
        {
            try
            {
                var products = await _myDbContext.Products.ToListAsync();

                foreach (var product in products)
                {
                    var productDetails = await _myDbContext.ProductDetails
                        .Where(pd => pd.ProductId == product.Id && pd.Status == 1)
                        .ToListAsync();

                    // Tính tổng số lượng của tất cả sản phẩm chi tiết
                    int? totalQuantity = productDetails.Sum(pd => pd.Quantity);
                    //if (totalQuantity>0)
                    //{
                    //    product.Status = true;
                    //}
                    //else
                    //{
                    //product.Status = false;
                    //}

                    // Cập nhật số lượng sản phẩm chính
                    product.AvailableQuantity = totalQuantity;

                    // Cập nhật vào cơ sở dữ liệu
                    _myDbContext.Products.Update(product);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _myDbContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                // Xử lý exception tại đây (log, throw, ...)
            }
        }
    }
}