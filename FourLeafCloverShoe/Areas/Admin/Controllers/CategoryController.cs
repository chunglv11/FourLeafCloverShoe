using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _categoryService.Gets());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Category category)
        {
            var categories = await _categoryService.Gets();
            if (categories.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower()))
            {
                TempData["ErrorMessage"] = "Loại này đã có";
                return View();
            }
            else if (category.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var result = await _categoryService.Add(category);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditAsync(Guid Id)
        {
            return View(await _categoryService.GetById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category obj)
        {
            var cate = await _categoryService.Gets();
            if (cate.Any(c => c.Name == obj.Name))
            {
                TempData["ErrorMessage"] = "Loại này đã có";
                return View();
            }
            else if (obj.Name == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var s = await _categoryService.GetById(obj.Id);
                s.Name = obj.Name;
                var result = await _categoryService.Update(s);
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
            var result = await _categoryService.Delete(Id);
            return RedirectToAction("Index", "Category") ;
        }
    }
}
