using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IProductService
    {
        public Task<bool> Add(Product obj);
        public Task<bool> AddMany(List<Product> lstobj);
        public Task<bool> Update(Product obj);
        public Task<bool> Delete(Guid Id);
        public Task<Product> GetById(Guid Id);
        public Task<List<Product>> Gets();
        public Task UpdateStatusQuantity();
    }
}
