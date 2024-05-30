using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class ColorsService:IColorsService
    {
        private readonly MyDbContext _myDbContext;

        public ColorsService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Colors obj)
        {
            try
            {
                await _myDbContext.Colors.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Colors> lstobj)
        {
            try
            {
                await _myDbContext.Colors.AddRangeAsync(lstobj);
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
                    _myDbContext.Colors.Remove(obj);
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

        public async Task<Colors> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Colors.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Colors();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Colors();
            }
        }

        public async Task<List<Colors>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Colors.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Colors>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Colors>();
            }
        }

        public async Task<bool> Update(Colors obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.ColorName = obj.ColorName;
                    objFromDb.ColorCode = obj.ColorCode;
                    _myDbContext.Colors.Update(objFromDb);
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
