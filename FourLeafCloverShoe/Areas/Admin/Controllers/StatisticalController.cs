using FourLeafCloverShoe.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]

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
            return View(statistical);
        }
    }
}
