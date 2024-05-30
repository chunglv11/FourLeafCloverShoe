using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class AddressService : IAddressService
    {
        private readonly MyDbContext _myDbContext;

        public AddressService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Address obj)
        {
            try
            {
                await _myDbContext.Address.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Address> lstobj)
        {
            try
            {
                await _myDbContext.Address.AddRangeAsync(lstobj);
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
               var obj =  await GetById(Id);
                if (obj!=null)
                {
                    _myDbContext.Address.Remove(obj);
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

        public async Task<Address> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Address.FindAsync(Id);
                if (obj != null)
                {
                   
                    return obj;
                }
                return new Address();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Address();
            }
        }

        public async Task<List<Address>> GetByUserId(string userId)
        {
            try
            {
                var obj =  _myDbContext.Address.Where(c=>c.UserId== userId).ToList();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Address>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Address>();
            }
        }

        public async Task<List<Address>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Address.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Address>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Address>();
            }
        }

        public async Task<bool> SetDefault(Guid Id)
        {
            try
            {
                var lstAddress = await GetByUserId((await _myDbContext.Address.ToListAsync()).FirstOrDefault(c => c.Id == Id).UserId);
                foreach (var item in lstAddress)
                {
                    if (item.Id==Id)
                    {

                    item.IsDefault=true;
                    }
                    else
                    {
                        item.IsDefault = false;
                    }
                }
                _myDbContext.Address.UpdateRange(lstAddress);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> Update(Address obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.RecipientName = obj.RecipientName;
                    objFromDb.RecipientPhone = obj.RecipientPhone;
                    objFromDb.ProvinceID = obj.ProvinceID;
                    objFromDb.ProvinceName = obj.ProvinceName;
                    objFromDb.DistrictID = obj.DistrictID;
                    objFromDb.DistrictName = obj.DistrictName;
                    objFromDb.WardCode = obj.WardCode;
                    objFromDb.WardName = obj.WardName;
                    objFromDb.IsDefault = obj.IsDefault;
                    objFromDb.Description = obj.Description;
                    _myDbContext.Address.Update(objFromDb);
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
