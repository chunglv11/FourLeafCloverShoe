using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IVoucherService
    {
        public Task<bool> Add(Voucher obj);
        public Task<bool> AddMany(List<Voucher> lstobj);
        public Task<bool> Update(Voucher obj);
        public Task<bool> Delete(Guid Id);
        public Task<Voucher> GetById(Guid Id);
        public Task<List<Voucher>> Gets();
    }
}
