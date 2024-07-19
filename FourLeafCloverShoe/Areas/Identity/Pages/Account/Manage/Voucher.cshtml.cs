using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class VoucherModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly IUserVoucherService _userVoucherService;

        private readonly IVoucherService _voucherService;
        public VoucherModel(UserManager<User> userManager, IUserVoucherService userVoucherService, IVoucherService voucherService)
        {
            _userManager = userManager;
            _userVoucherService = userVoucherService;
            _voucherService = voucherService;
        }

        public IEnumerable<Voucher> ListVoucher { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["type"] = 0;
            ViewData["search"] = "";

            if (User.Identity.IsAuthenticated)
            {
                var listVcID = await _userVoucherService.GetAllByUserId(user.Id);
                if (listVcID == null)
                {
                    TempData["Novoucher"] = "Danh sách voucher trống";
                }
                ListVoucher = await _voucherService.GetVouchersByIds(listVcID);
                return Page();
            }
            else
            {
                TempData["Userlogin"] = "Bạn cần phải đăng nhập";
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync(int type, string search)
        {
            ViewData["type"] = type;
            ViewData["search"] = search;

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (User.Identity.IsAuthenticated)
            {
                var listVcID = await _userVoucherService.GetAllByUserId(user.Id);
                if (listVcID == null)
                {
                    TempData["Novoucher"] = "Danh sách voucher trống";
                    return Page();
                }

                var vouchers = await _voucherService.GetVouchersByIds(listVcID);
      
                if (type == 0)
                {
                    ListVoucher = vouchers;
                }
                else if (type == 1)
                {
                    ListVoucher = vouchers.Where(c => c.Status == 1);
                    if (!ListVoucher.Any())
                    {
                        TempData["Novoucher"] = "Danh sách voucher chưa dùng trống";
                    }
                }
                else if (type == 2)
                {
                    ListVoucher = vouchers.Where(c => c.Status == 2);
                    if (!ListVoucher.Any())
                    {
                        TempData["Novoucher"] = "Danh sách voucher đã dùng trống";
                    }
                }
                if (!string.IsNullOrEmpty(search))
                {
                    ListVoucher = ListVoucher.Where(v => v.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || v.VoucherCode.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                return Page();
            }
            else
            {
                TempData["Userlogin"] = "Bạn cần phải đăng nhập";
                return RedirectToPage("/Account/Login");
            }
        }

    }
}
