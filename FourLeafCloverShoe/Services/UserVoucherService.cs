using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class UserVoucherService : IUserVoucherService
    {
        private readonly MyDbContext _myDbContext;

        public UserVoucherService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(UserVoucher obj)
        {
            try
            {
                await _myDbContext.UserVouchers.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<UserVoucher> lstobj)
        {
            try
            {
                await _myDbContext.UserVouchers.AddRangeAsync(lstobj);
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
                    _myDbContext.UserVouchers.Remove(obj);
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

        public async Task<UserVoucher> GetById(Guid Id)
        {
            try
            {
                var obj = (await _myDbContext.UserVouchers.Include(c => c.Vouchers).ToListAsync()).FirstOrDefault(c=>c.Id==Id);
                if (obj != null)
                {

                    return obj;
                }
                return new UserVoucher();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new UserVoucher();
            }
        }

        public async Task<List<UserVoucher>> GetByUserId(string userId)
        {
            try
            {
                var userVouchers = await _myDbContext.UserVouchers
                                        .Include(c => c.Vouchers).ToListAsync();
                var lstObjValid = userVouchers.Where(c=>c.UserId==userId&&c.Status==1&&c.Vouchers.Status==1&&c.Vouchers.StartDate<=DateTime.Now&&c.Vouchers.EndDate>DateTime.Now&&c.Vouchers.Quantity>0);
                return lstObjValid.ToList();

            }
            catch (Exception)
            {

                return new List<UserVoucher>();
            }
        }

        public async Task<List<UserVoucher>> GetByVoucherId(Guid voucherId)
        {
            try
            {
                var userVouchers = await _myDbContext.UserVouchers
                                        .Include(c => c.Vouchers).ToListAsync();
                var lstObjValid = userVouchers.Where(c => c.VoucherId == voucherId);
                return lstObjValid.ToList();

            }
            catch (Exception)
            {

                return new List<UserVoucher>();
            }
        }

        public async Task<List<UserVoucher>> Gets()
        {
            try
            {
                var obj = await _myDbContext.UserVouchers.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<UserVoucher>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<UserVoucher>();
            }
        }

        public async Task<bool> Update(UserVoucher obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Status = obj.Status;
                    _myDbContext.UserVouchers.Update(objFromDb);
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
