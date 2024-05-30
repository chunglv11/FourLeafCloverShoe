using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _brandService.Gets()).OrderBy(c => c.Name));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Brand s)
        {
            var Brand = await _brandService.Gets();
            if (Brand.Any(c => c.Name.Trim().ToLower() == s.Name.Trim().ToLower()))
            {
                TempData["ErrorMessage"] = "Hãng đã tồn tại";
                return View();
            }
            else if (s.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var result = await _brandService.Add(s);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(Guid Id)
        {

            return View(await _brandService.GetById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, Brand obj)
        {
            var Brand = await _brandService.Gets();
            if (Brand.Any(c => c.Name == obj.Name))
            {
                TempData["ErrorMessage"] = "Hãng này đã có trong hệ thống";
                return View();
            }
            else if (obj.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var s = await _brandService.GetById(Id);
                s.Name = obj.Name;
                var result = await _brandService.Update(s);
                if (result)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _brandService.Delete(Id);
            if (result)
            {
                TempData["SuccessMessage"] = "Xóa hãng thành công";
            }
            else
            {
                TempData["ErrorMessage"] = " Xóa thất bại ";
            }
            return RedirectToAction("Index");
        }
    }
}
