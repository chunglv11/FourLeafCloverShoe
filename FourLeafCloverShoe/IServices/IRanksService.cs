using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IRanksService
    {
        public Task<bool> Add(Ranks obj);
        public Task<bool> AddMany(List<Ranks> lstobj);
        public Task<bool> Update(Ranks obj);
        public Task<bool> Delete(Guid Id);
        public Task<Ranks> GetById(Guid Id);
        public Task<List<Ranks>> Gets();
    }
}
