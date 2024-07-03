using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Controllers
{
    public class VouchersController : Controller
    {

        private readonly UserManager<User> _userManager;

        private readonly IUserVoucherService _userVoucherService;

        private readonly IVoucherService _voucherService;
        public VouchersController(UserManager<User> userManager, IUserVoucherService userVoucherService, IVoucherService voucherService)
        {
            _userManager = userManager;
            _userVoucherService = userVoucherService;
            _voucherService = voucherService;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                
                var userVouser = await _userVoucherService.GetByUserId(user.Id);
                if (userVouser != null)
                {
                    ViewBag.noti = "Bạn chưa có voucher nào cả";
                }
                var voucherIds = userVouser
                 .Select(uv => uv.VoucherId)
                 .Where(id => id.HasValue)
                 .Select(id => id.Value)
                 .ToList();
                var vouchers = await _voucherService.GetVouchersByIds(voucherIds);
                return View(vouchers);
            }else
            {
                ViewBag.noti = "Bạn cần đăng nhập để xem vouchers";
                return View();
            }
        }
    }
}
