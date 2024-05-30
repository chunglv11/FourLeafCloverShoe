using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class RanksService : IRanksService
    {
        private readonly MyDbContext _myDbContext;

        public RanksService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Ranks obj)
        {
            try
            {
                await _myDbContext.Ranks.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Ranks> lstobj)
        {
            try
            {
                await _myDbContext.Ranks.AddRangeAsync(lstobj);
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
                    _myDbContext.Ranks.Remove(obj);
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

        public async Task<Ranks> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Ranks.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Ranks();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Ranks();
            }
        }

        public async Task<List<Ranks>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Ranks.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Ranks>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Ranks>();
            }
        }

        public async Task<bool> Update(Ranks obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Name = obj.Name;
                    _myDbContext.Ranks.Update(objFromDb);
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
