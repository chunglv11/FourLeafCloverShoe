using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IMaterialService
    {
        public Task<bool> Add(Material obj);
        public Task<bool> AddMany(List<Material> lstobj);
        public Task<bool> Update(Material obj);
        public Task<bool> Delete(Guid Id);
        public Task<Material> GetById(Guid Id);
        public Task<List<Material>> Gets();
    }
}
