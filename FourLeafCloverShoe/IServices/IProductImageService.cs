using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IProductImageService
    {
        public Task<bool> Add(ProductImages obj);
        public Task<bool> AddMany(List<ProductImages> lstobj);
        public Task<bool> Update(ProductImages obj);
        public Task<bool> Delete(Guid Id);
        public Task<ProductImages> GetById(Guid Id);
        public Task<List<ProductImages>> Gets();
    }
}
