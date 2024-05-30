using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IColorsService
    {
        public Task<bool> Add(Colors obj);
        public Task<bool> AddMany(List<Colors> lstobj);
        public Task<bool> Update(Colors obj);
        public Task<bool> Delete(Guid Id);
        public Task<Colors> GetById(Guid Id);
        public Task<List<Colors>> Gets();
    }
}
