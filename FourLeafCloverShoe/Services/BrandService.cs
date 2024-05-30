using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class BrandService : IBrandService
    {
        private readonly MyDbContext _myDbContext;

        public BrandService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Brand obj)
        {
            try
            {
                await _myDbContext.Brands.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Brand> lstobj)
        {
            try
            {
                await _myDbContext.Brands.AddRangeAsync(lstobj);
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
                    _myDbContext.Brands.Remove(obj);
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

        public async Task<Brand> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Brands.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Brand();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Brand();
            }
        }

        public async Task<List<Brand>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Brands.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Brand>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Brand>();
            }
        }

        public async Task<bool> Update(Brand obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Name = obj.Name;
                    _myDbContext.Brands.Update(objFromDb);
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
