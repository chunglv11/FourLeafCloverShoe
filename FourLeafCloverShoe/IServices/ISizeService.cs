using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface ISizeService
    {
        public Task<bool> Add(Size obj);
        public Task<bool> AddMany(List<Size> lstobj);
        public Task<bool> Update(Size obj);
        public Task<bool> Delete(Guid Id);
        public Task<Size> GetById(Guid Id);
        public Task<List<Size>> Gets();
    }
}
