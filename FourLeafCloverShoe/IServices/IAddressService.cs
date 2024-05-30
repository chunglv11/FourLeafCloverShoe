using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IAddressService
    {
        public Task<bool> Add(Address obj);
        public Task<bool> AddMany(List<Address> lstobj);
        public Task<bool> Update(Address obj);
        public Task<bool> Delete(Guid Id);
        public Task<bool> SetDefault(Guid Id);
        public Task<Address> GetById(Guid Id);
        public Task<List<Address>> GetByUserId(string userId);
        public Task<List<Address>> Gets();
    }
}
