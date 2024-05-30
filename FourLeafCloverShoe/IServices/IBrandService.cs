using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IBrandService
    {
        public Task<bool> Add(Brand obj);
        public Task<bool> AddMany(List<Brand> lstobj);
        public Task<bool> Update(Brand obj);
        public Task<bool> Delete(Guid Id);
        public Task<Brand> GetById(Guid Id);
        public Task<List<Brand>> Gets();
    }
}
