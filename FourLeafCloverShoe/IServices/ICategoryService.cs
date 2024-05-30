using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface ICategoryService
    {
        public Task<bool> Add(Category obj);
        public Task<bool> AddMany(List<Category> lstobj);
        public Task<bool> Update(Category obj);
        public Task<bool> Delete(Guid Id);
        public Task<Category> GetById(Guid Id);
        public Task<List<Category>> Gets();
    }
}
