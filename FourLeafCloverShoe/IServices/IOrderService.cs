using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.IServices
{
    public interface IOrderService
    {
        public Task<bool> Add(Order obj);
        public Task<bool> AddMany(List<Order> lstobj);
        public Task<bool> Update(Order obj);
        public Task<bool> Delete(Guid Id);
        public Task<bool> DeleteMany(List<Order> lstobj);
        public Task<Order> GetById(Guid Id);
        public Task<List<Order>> Gets();
        public Task<bool> UpdateOrderStatus(Guid idOrder, int? Status, string? IdStaff);
        
        public bool ThanhCong(Guid idHoaDon, string? idNhanVien);
        public Task<bool> HuyHD(Guid idhd, string idnv);

        public Task<bool> UpdateRank(int? point);
        public bool UpdateGhiChuHD(Guid idhd, string? idnv, string ghichu);

    }
}
