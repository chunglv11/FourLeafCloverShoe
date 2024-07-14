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
        public async Task<IActionResult> Index(int status, string search)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                ViewBag.tieude = "Danh sách vouchers của bạn";
                if (status == 0)
                {
                    var listVcID = await _userVoucherService.GetAllByUserId(user.Id);
                    if (listVcID == null)
                    {
                        ViewBag.noti = "Bạn chưa có voucher nào cả";
                    }
                    var vouchers = await _voucherService.GetVouchersByIds(listVcID);
                 
                    return View(vouchers);
                }

                if (status == 1)
                {
                    var listVcID = await _userVoucherService.GetAllByUserIdActive(user.Id);
                    if (listVcID == null)
                    {
                        ViewBag.noti = "Bạn chưa có voucher nào chưa dùng cả";
                    }
                    var vouchers = await _voucherService.GetVouchersByIds(listVcID);
                    ViewBag.tieude = "Danh sách vouchers chưa sử dụng của bạn";

                    return View(vouchers);
                }

                if (status != 1 && status !=0 && status != null)
                {
                    var listVcID = await _userVoucherService.GetAllByUserIdUnActive(user.Id);
                    if (listVcID == null)
                    {
                        ViewBag.noti = "Bạn chưa có voucher nào đã dùng cả";
                    }
                    var vouchers = await _voucherService.GetVouchersByIds(listVcID);
                    ViewBag.tieude = "Danh sách vouchers đã sử dụng của bạn";

                    return View(vouchers);
                }
                var vouchersnull = new Voucher();
                return View(vouchersnull);
            }
            else
            {
                ViewBag.noti = "Bạn cần đăng nhập để xem vouchers";
                var vouchers = new Voucher();
                return View();
            }
        }
    }
}
