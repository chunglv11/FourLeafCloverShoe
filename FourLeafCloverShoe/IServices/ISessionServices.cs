using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface ISessionServices
    {
        public List<CartItem> GetCartItems(ISession session, string key);
        public  Task SetCartItems(ISession session, string key, List<CartItem> CartItems);
        public bool CheckExistProduct(Guid id, List<CartItem> CartItems);
        public  Task<CartItem> GetItemByProductDetailId(Guid id, List<CartItem> CartItems);
    }
}
