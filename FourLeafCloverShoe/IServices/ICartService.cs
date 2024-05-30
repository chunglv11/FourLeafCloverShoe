using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface ICartService
    {
        public Task<bool> Add(Cart obj);
        public Task<bool> AddMany(List<Cart> lstobj);
        public Task<bool> Update(Cart obj);
        public Task<bool> Delete(Guid Id);
        public Task<Cart> GetById(Guid Id);
        public Task<Cart> GetByUserId(string userId);
        public Task<List<Cart>> Gets();
    }
}
