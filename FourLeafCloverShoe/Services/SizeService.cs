using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class SizeService : ISizeService
    {
        private readonly MyDbContext _myDbContext;

        public SizeService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Size obj)
        {
            try
            {
                await _myDbContext.Sizes.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Size> lstobj)
        {
            try
            {
                await _myDbContext.Sizes.AddRangeAsync(lstobj);
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
                    _myDbContext.Sizes.Remove(obj);
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

        public async Task<Size> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Sizes.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Size();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Size();
            }
        }

        public async Task<List<Size>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Sizes.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Size>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Size>();
            }
        }

        public async Task<bool> Update(Size obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Name = obj.Name;
                    _myDbContext.Sizes.Update(objFromDb);
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
