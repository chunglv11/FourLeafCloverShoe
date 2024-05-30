using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;

namespace FourLeafCloverShoe.IServices
{
    public interface IOrderItemService
    {
        public Task<bool> Add(OrderItem obj);
        public Task<bool> AddMany(List<OrderItem> lstobj);
        public Task<bool> Update(OrderItem obj);
        public Task<bool> Delete(Guid Id);
        public Task<bool> DeleteMany(List<OrderItem> lstobj);
        public Task<OrderItem> GetById(Guid Id);
        public Task<List<OrderDetailViewModel>> GetByIdOrder(Guid IdOrder);
        public Task<List<OrderItem>> Gets();
    }
}
