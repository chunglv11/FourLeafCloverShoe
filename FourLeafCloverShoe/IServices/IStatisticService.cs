using FourLeafCloverShoe.Share.ViewModels;

namespace FourLeafCloverShoe.IServices
{
    public interface IStatisticService
    {
        public Task<StatisticalViewModal> GetStatistics(int? month, int? year);
    }
}
