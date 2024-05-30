using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class MaterialService: IMaterialService
    {
        private readonly MyDbContext _myDbContext;

        public MaterialService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Material obj)
        {
            try
            {
                await _myDbContext.materials.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Material> lstobj)
        {
            try
            {
                await _myDbContext.materials.AddRangeAsync(lstobj);
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
                    _myDbContext.materials.Remove(obj);
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

        public async Task<Material> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.materials.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Material();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Material();
            }
        }

        public async Task<List<Material>> Gets()
        {
            try
            {
                var obj = await _myDbContext.materials.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Material>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Material>();
            }
        }

        public async Task<bool> Update(Material obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Name = obj.Name;
                    _myDbContext.materials.Update(objFromDb);
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
