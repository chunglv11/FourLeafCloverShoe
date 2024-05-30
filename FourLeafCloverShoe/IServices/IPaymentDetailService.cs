using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IPaymentDetailService
    {
        public Task<bool> Add(PaymentDetail obj);
        public Task<bool> Update(PaymentDetail obj);
        public Task<bool> Delete(Guid Id);
        public Task<PaymentDetail> GetById(Guid Id);
        public Task<List<PaymentDetail>> Gets();
    }
}
