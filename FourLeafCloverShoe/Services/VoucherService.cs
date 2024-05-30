using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly MyDbContext _myDbContext;

        public VoucherService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Voucher obj)
        {
            try
            {
                obj.CreateDate = DateTime.Now;
                await _myDbContext.Vouchers.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Voucher> lstobj)
        {
            try
            {
                await _myDbContext.Vouchers.AddRangeAsync(lstobj);
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
                    _myDbContext.Vouchers.Remove(obj);
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

        public async Task<Voucher> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Vouchers.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Voucher();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Voucher();
            }
        }

        public async Task<List<Voucher>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Vouchers
                   .Include(c=>c.UserVouchers)
                        .ThenInclude(c=>c.Users)
                            .ThenInclude(c=>c.Ranks)
                    .ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Voucher>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Voucher>();
            }
        }

        public async Task<bool> Update(Voucher obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.VoucherCode = obj.VoucherCode;
                    objFromDb.Title = obj.Title;
                    objFromDb.Quantity = obj.Quantity;
                    objFromDb.VoucherValue = obj.VoucherValue;
                    objFromDb.VoucherType = obj.VoucherType;
                    objFromDb.MinimumOrderValue = obj.MinimumOrderValue;
                    objFromDb.MaximumOrderValue = obj.MaximumOrderValue;
                    objFromDb.StartDate = obj.StartDate;
                    objFromDb.EndDate = obj.EndDate;
                    objFromDb.Status = obj.Status;
                    _myDbContext.Vouchers.Update(objFromDb);
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
