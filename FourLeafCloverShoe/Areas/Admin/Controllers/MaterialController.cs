using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MaterialController : Controller
    {
       
        private     IMaterialService  _imaterialService;

        public MaterialController(IMaterialService materialService)
        {
            _imaterialService = materialService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _imaterialService.Gets()).OrderBy(c => c.Name));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Material s)
        {
            var size = await _imaterialService.Gets();
            if (size.Any(c => c.Name == s.Name))
            {
                TempData["ErrorMessage"] = "Chất Liệu đã có";
                return View();
            }
            else if (s.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var result = await _imaterialService.Add(s);
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

            return View(await _imaterialService.GetById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, Size obj)
        {
            var size = await _imaterialService.Gets();
            if (size.Any(c => c.Name == obj.Name))
            {
                TempData["ErrorMessage"] = "Chất Liệu đã có";
                return View();
            }
            else if (obj.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var s = await _imaterialService.GetById(Id);
                s.Name = obj.Name;
                var result = await _imaterialService.Update(s);
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
            var result = await _imaterialService.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
