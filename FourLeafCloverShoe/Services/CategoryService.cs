using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly MyDbContext _myDbContext;

        public CategoryService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Category obj)
        {
            try
            {
                await _myDbContext.Categories.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Category> lstobj)
        {
            try
            {
                await _myDbContext.Categories.AddRangeAsync(lstobj);
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
                    _myDbContext.Categories.Remove(obj);
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

        public async Task<Category> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Categories.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Category();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Category();
            }
        }

        public async Task<List<Category>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Categories.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Category>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Category>();
            }
        }

        public async Task<bool> Update(Category obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Name = obj.Name;
                    _myDbContext.Categories.Update(objFromDb);
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