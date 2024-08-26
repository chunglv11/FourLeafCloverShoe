using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc;
using FourLeafCloverShoe.Helper;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text.RegularExpressions;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAreaAuthorization]
    public class ColorController : Controller
    {
        private IColorsService _colorService;

        public ColorController(IColorsService colorService)
        {
            _colorService = colorService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _colorService.Gets()).OrderBy(c => c.ColorName));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Colors s)
        {
            if (s.ColorCode == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            var color = await _colorService.Gets();
            if(s.ColorName == null)
            {
                TempData["ErrorMessage"] = "Bạn cần nhập tên màu";
                return View();
            }
            if (color.Any(c => c.ColorName != null && c.ColorName.Trim().ToLower() == s.ColorName.Trim().ToLower()))
            {
                TempData["ErrorMessage"] = "Màu đã có vui lòng thêm màu khác";
                return View();
            }
            else if (IsValidHexColor(s.ColorCode.Trim()) == false)
            {
                TempData["ErrorMessage"] = "Mã màu bạn thêm không đúng định dạng";
                return View();
            }
            else if (color.Any(c => c.ColorCode != null && c.ColorCode.Trim().ToLower() == s.ColorCode.Trim().ToLower()))
            {
                TempData["ErrorMessage"] = "Mã Màu đã có vui lòng thêm màu khác";
                return View();
            }
            else
            {
                s.Id = Guid.NewGuid();
                s.ColorName = s.ColorName.Trim();
                s.ColorCode = s.ColorCode.Trim();
                var result = await _colorService.Add(s);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        //[HttpPost("CreateinProductDetail")]
        //public async Task<IActionResult> Create(string colorName, string colorCode, string idProduct)
        //{

        //    var color = await _colorService.Gets();
        //    if (colorName == null)
        //    {
        //        TempData["ErrorMessage"] = "Bạn cần nhập tên màu";
        //        return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        //    }
        //    if (colorCode == null)
        //    {
        //        TempData["ErrorMessage"] = "Không được để trống";
        //        return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        //    }
        //    if (color.Any(c => c.ColorName != null && c.ColorName.Trim().ToLower() == colorName.Trim().ToLower()))
        //    {
        //        TempData["ErrorMessage"] = "Màu đã có vui lòng thêm màu khác";
        //        return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        //    }
        //    else if (IsValidHexColor(colorCode.Trim()) == false)
        //    {
        //        TempData["ErrorMessage"] = "Mã màu bạn thêm không đúng định dạng";
        //        return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        //    }
        //    else if (color.Any(c => c.ColorCode != null && c.ColorCode.Trim().ToLower() == colorCode.Trim().ToLower()))
        //    {
        //        TempData["ErrorMessage"] = "Mã Màu đã có vui lòng thêm màu khác";
        //        return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        //    }
        //    else
        //    {
        //        var colornew = new Colors();
        //        colornew.Id = Guid.NewGuid();
        //        colornew.ColorName = colorName.Trim();
        //        colornew.ColorCode = colorCode.Trim();
        //        var result = await _colorService.Add(colornew);
        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Thêm thành công";
        //            return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        //        }
        //    }
        //    return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        //}

        private bool IsValidHexColor(string? colorCode)
        {
            string pattern = @"^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$";
            return Regex.IsMatch(colorCode, pattern);
        }

        public async Task<IActionResult> Edit(Guid Id)
        {

            return View(await _colorService.GetById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, Colors obj)
        {
            var color = await _colorService.Gets();
            if (obj.ColorName == null)
            {
                TempData["ErrorMessage"] = "Bạn cần nhập tên màu";
                return View();
            }
            if (color.Any(c => c.ColorName.Trim().ToLower() == obj.ColorName.Trim().ToLower() && c.Id != Id))
            {
                TempData["ErrorMessage"] = "Màu đã có";
                return View();
            }
            else if (IsValidHexColor(obj.ColorCode.Trim()) == false)
            {
                TempData["ErrorMessage"] = "Mã màu bạn thêm không đúng định dạng";
                return View();
            }
            else if (color.Any(c => c.ColorCode.Trim().ToLower() == obj.ColorCode.Trim().ToLower() && c.Id != Id))
            {
                TempData["ErrorMessage"] = "Mã Màu đã có vui lòng thêm màu khác";
                return View();
            }
            else if (obj.ColorName == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else if (obj.ColorCode == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View();
            }
            else
            {
                var s = await _colorService.GetById(Id);
                s.ColorName = obj.ColorName.Trim();
                s.ColorCode = obj.ColorCode.Trim();
                var result = await _colorService.Update(s);
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
            var result = await _colorService.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
