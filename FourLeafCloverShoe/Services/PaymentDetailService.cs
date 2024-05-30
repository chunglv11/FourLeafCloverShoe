using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class PaymentDetailService : IPaymentDetailService
    {
        private readonly MyDbContext _myDbContext;

        public PaymentDetailService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public async Task<bool> Add(PaymentDetail obj)
        {
            try
            {
                await _myDbContext.PaymentDetails.AddAsync(obj);
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
                    _myDbContext.PaymentDetails.Remove(obj);
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

        public async Task<PaymentDetail> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.PaymentDetails.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new PaymentDetail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new PaymentDetail();
            }
        }

        public async Task<List<PaymentDetail>> Gets()
        {
            try
            {
                var obj = await _myDbContext.PaymentDetails.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<PaymentDetail>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<PaymentDetail>();
            }
        }

        public async Task<bool> Update(PaymentDetail obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.IdOrder = obj.IdOrder;
                    objFromDb.IdPayment = obj.IdPayment;
                    objFromDb.TotalMoney = obj.TotalMoney;
                    objFromDb.Note = obj.Note;
                    objFromDb.Status = obj.Status;
                    _myDbContext.PaymentDetails.Update(objFromDb);
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
