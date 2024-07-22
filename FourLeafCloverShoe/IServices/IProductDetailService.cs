using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;

namespace FourLeafCloverShoe.IServices
{
    public interface IProductDetailService
    {
        public Task<bool> Add(ProductDetail obj);
        public Task<bool> AddMany(List<ProductDetail> lstobj);
        public Task<bool> Update(ProductDetail obj);
        public Task<bool> UpdateMany(List<ProductDetail> lstobj);
        public Task<bool> Delete(Guid Id);
        public Task<ProductDetail> GetById(Guid Id);
        public Task<List<ProductDetail>> GetByProductId(Guid ProductId);
        public Task<List<ProductDetail>> Gets();
        public Task<List<ProductDeailViewModel>> GetProductDetails();
        public Task<bool> UpdateQuantityById(Guid? productDetailId, int? quantity);
        public Task<bool> UpdateQuantityOrderFail(Guid productDetailId, int? quantity);
    }
}
