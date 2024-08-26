using FourLeafCloverShoe.Helper;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAreaAuthorization]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
