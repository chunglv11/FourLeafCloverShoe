using FourLeafCloverShoe.IServices;
using Microsoft.AspNetCore.Mvc;
using FourLeafCloverShoe.Helper;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]

    [AdminAreaAuthorization]
    public class StatisticalController : Controller
    {
        private readonly IStatisticService _statisticalService;

        public StatisticalController(IStatisticService statisticalService)
        {
                _statisticalService = statisticalService;
        }
        public async Task<IActionResult> Index(int? month, int? year)
        {
            var statistical = await _statisticalService.GetStatistics(month, year);
            ViewBag.SelectedMonth = month;
            ViewBag.SelectedYear = year;
            ViewBag.pTitle = "Thống kê";
            return View(statistical);
        }
    }
}
