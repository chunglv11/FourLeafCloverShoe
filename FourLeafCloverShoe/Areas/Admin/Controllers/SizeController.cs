using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _sizeService.Gets()).OrderBy(c=>c.Name));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Size s)
        {
            var size = await _sizeService.Gets();
            if (size.Any(c => c.Name == s.Name))
            {
                TempData["ErrorMessage"] = "Kích cỡ đã có";
                return View();
            }
            else if (s.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var result = await _sizeService.Add(s);
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

            return View(await _sizeService.GetById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, Size obj)
        {
            var size = await _sizeService.Gets();
            if (size.Any(c => c.Name == obj.Name))
            {
                TempData["ErrorMessage"] = "Kích cỡ đã có";
                return View();
            }
            else if (obj.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var s = await _sizeService.GetById(Id);
                s.Name = obj.Name;
                var result = await _sizeService.Update(s);
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
            var result = await _sizeService.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
