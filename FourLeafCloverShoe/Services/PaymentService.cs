using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly MyDbContext _myDbContext;

        public PaymentService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(PaymentType obj)
        {
            try
            {
                await _myDbContext.PaymentTypes.AddAsync(obj);
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
                    _myDbContext.PaymentTypes.Remove(obj);
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

        public async Task<PaymentType> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.PaymentTypes.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new PaymentType();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new PaymentType();
            }
        }

        public async Task<List<PaymentType>> Gets()
        {
            try
            {
                var obj = await _myDbContext.PaymentTypes.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<PaymentType>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<PaymentType>();
            }
        }

        public async Task<bool> Update(PaymentType obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Name = obj.Name;
                    objFromDb.Status = obj.Status;
                    _myDbContext.PaymentTypes.Update(objFromDb);
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
