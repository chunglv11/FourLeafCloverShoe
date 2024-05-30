using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;

namespace FourLeafCloverShoe.IServices
{
    public interface IRateService
    {
        public Task<bool> Add(Rate obj);
        public Task<bool> AddMany(List<Rate> lstobj);
        public Task<bool> Update(Rate obj);
        public Task<bool> Delete(Guid Id);
        public Task<Rate> GetById(Guid Id);
        public Task<List<RateViewModel>> GetByIdProduct(Guid IdPr);

        public Task<List<Rate>> Gets();
        public Task<bool> UpdateDanhGia(Guid id, Guid idhdct, float soSao, string? binhLuan, string? imageUrl);
    }
}
