using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface ICartItemItemService
    {
        public Task<bool> Add(CartItem obj);
        public Task<bool> AddMany(List<CartItem> lstobj);
        public Task<bool> Update(CartItem obj);
        public Task<bool> Delete(Guid Id);
        public Task<bool> DeleteMany(List<CartItem> lstobj);
        public Task<bool> DeleteByProductDetailId(Guid productDetailId);
        public Task<bool> UpdateQuantity(Guid cartItemId, int? newquantity);
        public Task<CartItem> GetById(Guid Id);
        public Task<List<CartItem>> Gets();
        public Task<List<CartItem>> GetsByUserId(string userId);
    }
}
