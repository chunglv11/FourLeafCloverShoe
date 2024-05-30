using FourLeafCloverShoe.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
        [Area("Admin")]

    //[AdminAreaAuthorization] // check admin or staff

    [Authorize(Roles ="Admin")]    
    
    public class DashboardController : Controller
    {
    //[Authorize("Create")]    //action được làm
        public IActionResult Index()
        {
            return View();
        }
    }
}
