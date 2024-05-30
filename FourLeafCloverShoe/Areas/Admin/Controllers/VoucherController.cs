using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.NetworkInformation;
using ZXing;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VoucherController : Controller
    {
        private IUserVoucherService _userVoucherService;
        private IVoucherService _voucherService;
        private readonly IRanksService _ranksService;
        private readonly UserManager<User> _userManager;

        public VoucherController(IVoucherService voucherService, IUserVoucherService userVoucherService, IRanksService ranksService, UserManager<User> userManager)
        {
            _userVoucherService = userVoucherService;
            _voucherService = voucherService;
            _ranksService = ranksService;
            _userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            return View((await _voucherService.Gets()).OrderByDescending(c => c.CreateDate));
        }
        public async Task<IActionResult> Create()
        {
            var ranks = (await _ranksService.Gets()).Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();
            ViewBag.Ranks = ranks;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(VoucherViewModel voucherViewModel)
        {
            var lstVoucherDb = await _voucherService.Gets();
            var ranks = (await _ranksService.Gets()).Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();
            ViewBag.Ranks = ranks;
            if (!lstVoucherDb.Any(c => c.VoucherCode == voucherViewModel.VoucherCode))
            {
                if (voucherViewModel.VoucherCode == null ||
                    voucherViewModel.VoucherCode == "" ||
                    voucherViewModel.MinimumOrderValue == null ||
                    voucherViewModel.MaximumOrderValue == null ||
                    voucherViewModel.Quantity == null ||
                    voucherViewModel.Status == null ||
                    voucherViewModel.StartDate == null ||
                    voucherViewModel.EndDate == null ||
                    voucherViewModel.VoucherType == null
                    )
                {
                    TempData["ErrorMessage"] = "Bạn phải nhập đầy đủ thông tin";
                    return View(voucherViewModel);
                }
                if (voucherViewModel.Ranks == null)
                {
                    TempData["ErrorMessage"] = "Bạn phải chọn rank";

                    return View(voucherViewModel);
                }
                if (voucherViewModel.StartDate >= voucherViewModel.EndDate)
                {
                    TempData["ErrorMessage"] = "Ngày bắt đầu lớn hơn ngày kết thúc";
                    return View(voucherViewModel);
                }
                if (voucherViewModel.StartDate <= DateTime.Now)
                {
                    TempData["ErrorMessage"] = "Ngày bắt đầu phải lớn hơn ht";
                    return View(voucherViewModel);
                }
                if (voucherViewModel.Quantity == 0 && voucherViewModel.Quantity == null)
                {
                    TempData["ErrorMessage"] = "Số lượng của vocher phải lơn hơn 0";
                    return View(voucherViewModel);
                }

                if (voucherViewModel.MaximumOrderValue > voucherViewModel.VoucherValue)
                {
                    TempData["ErrorMessage"] = "Giá trị giảm tối đa > giá trị";
                    return View(voucherViewModel);
                }
                var voucherNew = new Voucher()
                {
                    CreateDate = DateTime.Now,
                    StartDate = voucherViewModel.StartDate,
                    EndDate = voucherViewModel.EndDate,
                    Title = voucherViewModel.Title,
                    Status = voucherViewModel.Status,
                    MinimumOrderValue = voucherViewModel.MinimumOrderValue,
                    MaximumOrderValue = voucherViewModel.MaximumOrderValue,
                    Quantity = voucherViewModel.Quantity,
                    VoucherValue = voucherViewModel.VoucherValue,
                    VoucherCode = voucherViewModel.VoucherCode,
                    VoucherType = voucherViewModel.VoucherType
                };
                var result = await _voucherService.Add(voucherNew); // tao voucher
                if (result)
                {
                    var lstUserVoucher = new List<UserVoucher>();
                    var allUser = await _userManager.Users.ToListAsync();
                    var newList = allUser.Where(c => voucherViewModel.Ranks.Any(y => y == c.Ranks.Name));
                    foreach (var item in newList)
                    {
                        var newUserVoucher = new UserVoucher()
                        {
                            Status = 1,
                            UserId = item.Id,
                            VoucherId = voucherNew.Id
                        };
                        lstUserVoucher.Add(newUserVoucher);
                    }
                    voucherNew.UserVouchers = lstUserVoucher;
                    var resultAddUserVoucher = await _userVoucherService.AddMany(lstUserVoucher);
                    var updateVoucher = await _voucherService.Update(voucherNew);
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("Index");

                }
            }
            TempData["ErrorMessage"] = "Mã voucher bị trùng rồi";
            return View(voucherViewModel);
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            var findListUserVoucher = await _userVoucherService.GetByVoucherId(Id);
            if (findListUserVoucher.Count == 0)
            {
                await _voucherService.Delete(Id);
            }
            else
            {
                foreach (var item in findListUserVoucher)
                {
                    var deleteUserVoucher = await _userVoucherService.Delete(item.Id);
                }
                var result = await _voucherService.Delete(Id);
                TempData["SuccessMessage"] = "Xóa thành công";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> edit(Guid Id)
        {
            VoucherViewModel vcview = new VoucherViewModel();
            var findVoucher = await _voucherService.GetById(Id);
            vcview.Id = Id;
            vcview.VoucherCode = findVoucher.VoucherCode;
            vcview.Quantity = findVoucher.Quantity;
            vcview.Title = findVoucher.Title;
            vcview.VoucherValue = findVoucher.VoucherValue;
            vcview.VoucherType = findVoucher.VoucherType;
            vcview.MinimumOrderValue = findVoucher.MinimumOrderValue;
            vcview.MaximumOrderValue = findVoucher.MaximumOrderValue;
            vcview.CreateDate = findVoucher.CreateDate;
            vcview.StartDate = findVoucher.StartDate;
            vcview.Status = findVoucher.Status;
            vcview.EndDate = findVoucher.EndDate;
            var dsUser = await _userVoucherService.Gets();
            var ds = dsUser.Where(c => c.VoucherId == Id).ToList();
            List<string> user = new List<string>();
            foreach (var item in ds)
            {
                user.Add(item.UserId);
            }
            List<string> DanhsachRankcuaUser = new List<string>();
            foreach (var item in user)
            {
                var dsUsers = await _userManager.FindByIdAsync(item);
                var dsRank = await _ranksService.GetById(dsUsers.RankId);
                if (dsRank != null)
                {
                    DanhsachRankcuaUser.Add(dsRank.Name);
                }
            }
            var allRanks = await _ranksService.Gets();
            var otherRanks = allRanks.Select(r => r.Name).Except(DanhsachRankcuaUser).ToList();

            var ranks = otherRanks.Select(r => new SelectListItem
            {
                Text = r,
                Value = r
            }).ToList();
            ViewBag.Ranks = ranks;
            vcview.Ranks = DanhsachRankcuaUser.Distinct();
            return View(vcview);
        }
        [HttpPost]
        public async Task<IActionResult> edit(VoucherViewModel voucherViewModel)
        {
            var findVoucher = await _voucherService.GetById((Guid)voucherViewModel.Id);

            if (voucherViewModel.VoucherCode == null ||
                voucherViewModel.VoucherCode == "" ||
                voucherViewModel.MinimumOrderValue == null ||
                voucherViewModel.MaximumOrderValue == null ||
                voucherViewModel.Quantity == null ||
                voucherViewModel.Status == null ||
                voucherViewModel.StartDate == null ||
                voucherViewModel.EndDate == null ||
                voucherViewModel.VoucherType == null
                )
            {
                TempData["ErrorMessage"] = "Bạn phải nhập đầy đủ thông tin";
                return View(voucherViewModel);
            }
            if (voucherViewModel.VoucherType == 1)
            {
                if (voucherViewModel.VoucherValue > 100)
                {
                    TempData["ErrorMessage"] = "giá trị không được qúa 100%";
                    return View(voucherViewModel);
                }
            }
            if (voucherViewModel.Ranks == null)
            {
                TempData["ErrorMessage"] = "Bạn phải chọn rank";
                var ranks = (await _ranksService.Gets()).Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToList();
                ViewBag.Ranks = ranks;
                return View(voucherViewModel);
            }
            if (voucherViewModel.StartDate >= voucherViewModel.EndDate)
            {
                TempData["ErrorMessage"] = "Ngày bắt đầu lớn hơn ngày kết thúc";
                return View(voucherViewModel);
            }
            if (voucherViewModel.StartDate <= DateTime.Now)
            {
                TempData["ErrorMessage"] = "Ngày bắt đầu phải lớn hơn ht";
                return View(voucherViewModel);
            }
            if (voucherViewModel.Quantity == 0 && voucherViewModel.Quantity == null)
            {
                TempData["ErrorMessage"] = "Số lượng của vocher phải lơn hơn 0";
                return View(voucherViewModel);
            }
            if (voucherViewModel.VoucherType == 1)
            {
                voucherViewModel.MaximumOrderValue = (int)voucherViewModel.VoucherValue;
            }
            // UPDATE VOUCHER
            if (findVoucher != null)
            {
                findVoucher.CreateDate = voucherViewModel.CreateDate;
                findVoucher.StartDate = voucherViewModel.StartDate;
                findVoucher.EndDate = voucherViewModel.EndDate;
                findVoucher.Title = voucherViewModel.Title;
                findVoucher.Status = voucherViewModel.Status;
                findVoucher.MinimumOrderValue = voucherViewModel.MinimumOrderValue;
                findVoucher.MaximumOrderValue = voucherViewModel.MaximumOrderValue;
                findVoucher.Quantity = voucherViewModel.Quantity;
                findVoucher.VoucherValue = voucherViewModel.VoucherValue;
                findVoucher.VoucherCode = voucherViewModel.VoucherCode;
                findVoucher.VoucherType = voucherViewModel.VoucherType;
            }
            var result = await _voucherService.Update(findVoucher);
            // lay cac rank cua user cu sau do xoa     
            if (result)
            {
                var LastUserVc = await _userVoucherService.GetByVoucherId(findVoucher.Id);
                if (LastUserVc == null)
                {
                }
                foreach (var item in LastUserVc)
                {
                    await _userVoucherService.Delete(item.Id);
                }

                var allUser = _userManager.Users.ToList();
                List<Ranks> ranks = new List<Ranks>();
                foreach (var item in voucherViewModel.Ranks)
                {
                    var getallRank = await _ranksService.Gets();
                    var rank = getallRank.SingleOrDefault(c => c.Name == item);
                    ranks.Add(rank);
                }
                List<User> users = new List<User>();
                foreach (var item in ranks)
                {
                    var allUserbyrank = allUser.Where(c => c.RankId == item.Id);
                    users.AddRange(allUserbyrank);
                }
                List<UserVoucher> userVouchers = new List<UserVoucher>();
                foreach (var item in users)
                {
                    UserVoucher userVoucher = new UserVoucher();
                    userVoucher.VoucherId = voucherViewModel.Id;
                    userVoucher.UserId = item.Id;
                    userVoucher.Status = 1;
                    userVouchers.Add(userVoucher);
                }
                await _userVoucherService.AddMany(userVouchers);
                TempData["SuccessMessage"] = "Sửa thành công";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Sửa sản phẩm không thanh công";
            return View(voucherViewModel);

        }
    }
}
