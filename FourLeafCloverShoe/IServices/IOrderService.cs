using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IOrderService
    {
        public Task<bool> Add(Order obj);
        public Task<bool> AddMany(List<Order> lstobj);
        public Task<bool> Update(Order obj);
        public Task<bool> Delete(Guid Id);
        public Task<bool> DeleteMany(List<Order> lstobj);
        public Task<Order> GetById(Guid Id);
        public Task<List<Order>> Gets();
        public bool UpdateOrderStatus(Guid idOrder, int? Status, Guid? IdStaff);
    }
}
