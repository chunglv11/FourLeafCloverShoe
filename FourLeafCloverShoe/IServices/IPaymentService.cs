using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IPaymentService
    {
        public Task<bool> Add(PaymentType obj);
        public Task<bool> Update(PaymentType obj);
        public Task<bool> Delete(Guid Id);
        public Task<PaymentType> GetById(Guid Id);
        public Task<List<PaymentType>> Gets();
    }
}
