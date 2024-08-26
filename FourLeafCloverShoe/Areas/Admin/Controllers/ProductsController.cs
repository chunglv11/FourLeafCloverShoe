using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using ZXing;
using ZXing.QrCode.Internal;
using Size = FourLeafCloverShoe.Share.Models.Size;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISizeService _sizeService;
        private readonly IBrandService _brandService;
        private readonly IProductImageService _productImageService;
        private readonly ICategoryService _categoryService;
        private readonly IProductDetailService _productDetailService;
        private readonly IColorsService _colorsService;
        private readonly IMaterialService _materialsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<IFormFile> _lstIFormFile;

        public ProductsController(IProductService productService,
                                    ISizeService sizeService,
                                    IBrandService brandService,
                                    IProductImageService productImageService,
                                    IProductDetailService productDetailService,
                                    ICategoryService categoryService,
                                    IWebHostEnvironment webHostEnvironment,
                                    IColorsService colorsService,
                                    IMaterialService materialService
                                    )
        {
            _productService = productService;
            _sizeService = sizeService;
            _brandService = brandService;
            _productImageService = productImageService;
            _categoryService = categoryService;
            _productDetailService = productDetailService;
            _colorsService = colorsService;
            _materialsService = materialService;
            _lstIFormFile = new List<IFormFile>();
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            await _productService.UpdateStatusQuantity();
            var lstObj = await _productService.Gets();
            return View(lstObj.OrderByDescending(c => c.Status)); // trạng thái đang bán (True);
        }
        public async Task<IActionResult> CreateNewProduct()
        {
            List<SelectListItem> ListCategoryitems = new List<SelectListItem>();
            foreach (var obj in (await _categoryService.Gets()))
            {

                ListCategoryitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListCategoryitems = ListCategoryitems;
            List<SelectListItem> ListBranditems = new List<SelectListItem>();
            foreach (var obj in await _brandService.Gets())
            {
                ListBranditems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListBranditems = ListBranditems;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewProduct(Product product)
        {
            //var listTags = tags.Split(',').ToList(); // thêm vào chuỗi string 


            List<SelectListItem> ListCategoryitems = new List<SelectListItem>();
            // Giả sử myList là danh sách dữ liệu của bạn
            foreach (var obj in (await _categoryService.Gets()))
            {
                ListCategoryitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListCategoryitems = ListCategoryitems;
            List<SelectListItem> ListBranditems = new List<SelectListItem>();
            // Giả sử myList là danh sách dữ liệu của bạn
            foreach (var obj in await _brandService.Gets())
            {
                ListBranditems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListBranditems = ListBranditems;

            var pro = await _productService.Gets();
            var pros = pro.FirstOrDefault(p => p.ProductName.Trim().ToLower() == product.ProductName.Trim().ToLower());
            bool containsSpaceInMiddle = Regex.IsMatch(product.ProductName, @"^.+ .+$");
            if (pros != null && containsSpaceInMiddle == true)
            {
                ModelState.AddModelError("", "Vui lòng nhập tên sản phẩm khác");
                return View(product);
            }

            string imageList = HttpContext.Session.GetString("ImageList");
            if (ModelState.IsValid)
            {
                if (imageList == null)
                {
                    // Trả về view với thông báo lỗi nếu ModelState không hợp lệ hoặc không có tệp nào được tải lên
                    ModelState.AddModelError("", "Vui lòng thêm ảnh.");
                    return View(product);
                }

                product.CreateAt = DateTime.Now;
                product.Description = product.Description;
                product.ProductCode = GetInitials(product.ProductName) + DateTime.Now.ToString("yyMMddHHmmss");
                product.AvailableQuantity = 0;
                product.Status = product.Status;
                var result = await _productService.Add(product);
                if (result)
                {
                    List<string> imageListAsList = imageList.Split(';').ToList();
                    foreach (var item in imageListAsList)
                    {

                        var createProductImageResult = await _productImageService.Add(new ProductImages() { ProductId = product.Id, ImageUrl = item });
                        // Cập nhật đường dẫn hình ảnh cho sản phẩm

                    }
                    HttpContext.Session.Remove("ImageList");
                    return RedirectToAction("Index", "Products", new { @area = "Admin" });
                }

            }
            // Trả về view với thông báo lỗi nếu ModelState không hợp lệ hoặc không có tệp nào được tải lên
            ModelState.AddModelError("", "Vui lòng nhập đầy đủ các trường");
            return View(product);
        }
        public async Task<IActionResult> EditProduct(Guid productId) // edit product
        {
            List<SelectListItem> ListCategoryitems = new List<SelectListItem>();
            // Giả sử myList là danh sách dữ liệu của bạn
            foreach (var obj in (await _categoryService.Gets()))
            {
                ListCategoryitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListCategoryitems = ListCategoryitems;
            List<SelectListItem> ListBranditems = new List<SelectListItem>();
            // Giả sử myList là danh sách dữ liệu của bạn
            foreach (var obj in await _brandService.Gets())
            {
                ListBranditems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListBranditems = ListBranditems;
            return View(await _productService.GetById(productId));
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {

            List<SelectListItem> ListCategoryitems = new List<SelectListItem>();
            // Giả sử myList là danh sách dữ liệu của bạn
            foreach (var obj in (await _categoryService.Gets()))
            {
                ListCategoryitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListCategoryitems = ListCategoryitems;
            List<SelectListItem> ListBranditems = new List<SelectListItem>();
            // Giả sử myList là danh sách dữ liệu của bạn
            foreach (var obj in await _brandService.Gets())
            {
                ListBranditems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListBranditems = ListBranditems;
            string imageList = HttpContext.Session.GetString("ImageList");
            if (ModelState.IsValid)
            {

                var productDb = await _productService.GetById(product.Id);
                productDb.CategoryId = product.CategoryId;
                productDb.ProductName = product.ProductName;
                productDb.BrandId = product.BrandId;
                productDb.Description = product.Description;
                productDb.UpdateAt = DateTime.Now;
                productDb.Status = product.Status;
                if (imageList == null && productDb.ProductImages.Count() == 0) // check ảnh
                {
                    // Trả về view với thông báo lỗi nếu ModelState không hợp lệ hoặc không có tệp nào được tải lên
                    ModelState.AddModelError("", "Ảnh sản phẩm tối thiểu là 1, vui lòng thêm ảnh.");
                    return View(product);
                }
                if (imageList != null)
                {
                    List<string> imageListAsList = imageList.Split(';').ToList();
                    foreach (var item in imageListAsList)
                    {
                        var createProductImageResult = await _productImageService.Add(new ProductImages() { ProductId = product.Id, ImageUrl = item });
                        // Cập nhật đường dẫn hình ảnh cho sản phẩm

                    }
                }
                var result = await _productService.Update(productDb);
                if (result)
                {

                    HttpContext.Session.Remove("ImageList");
                    return RedirectToAction("Index", "Products", new { @area = "Admin" });
                }

            }

            // Trả về view với thông báo lỗi nếu ModelState không hợp lệ hoặc không có tệp nào được tải lên
            ModelState.AddModelError("", "Vui lòng nhập đầy đủ các trường");
            return RedirectToAction("Edit", null, new { @productId = product.Id });
        }

        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var result = await _productService.Delete(productId);
            return RedirectToAction("Index", "Products", new { @area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName).Trim();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageList = HttpContext.Session.GetString("ImageList");
            var updatedImageList = imageList == null ? $"/images/{fileName}" : $"{imageList};/images/{fileName}";

            // Cập nhật danh sách ảnh trong Session
            HttpContext.Session.SetString("ImageList", updatedImageList);

            return Ok();
        }
        [HttpPost]
        public async Task<bool> RemoveImageAsync(string productImageId)
        {
            if (productImageId != null)
            {
                var id = Guid.Parse(productImageId);
                var result = await _productImageService.Delete(id);
                return result;
            }
            return false;
        }
        static string GetInitials(string input) // tạo productcode 
        {
            string[] words = input.Split(' ');
            string initials = "";

            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    initials += word[0];
                }
            }

            return initials.ToUpper(); // Chuyển thành chữ hoa
        }

        public async Task<IActionResult> ProductDetail(Guid productId)
        {
            await _productService.UpdateStatusQuantity();
            var lstObj = await _productDetailService.GetByProductId(productId);
            ViewBag.productId = productId;
            return View(lstObj);
        }
        public async Task<IActionResult> CreateProductDetail(Guid productId)
        {
            List<SelectListItem> ListSizeitems = new List<SelectListItem>();
            List<SelectListItem> ListColorsitems = new List<SelectListItem>();
            List<SelectListItem> ListMaterialsitems = new List<SelectListItem>();

            var productDetails = await _productDetailService.GetByProductId(productId);

            foreach (var obj in (await _sizeService.Gets()).OrderBy(c => c.Name))
            {
                //if (productDetails.FirstOrDefault(p => p.SizeId == obj.Id) == null)
                //{
                ListSizeitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
                //}
            }
            foreach (var obj in (await _colorsService.Gets()).OrderBy(c => c.ColorName))
            {
                ListColorsitems.Add(new SelectListItem()
                {
                    Text = obj.ColorName,
                    Value = obj.Id.ToString(),
                });
            }
            foreach (var obj in (await _materialsService.Gets()).OrderBy(c => c.Name))
            {
                //if (productDetails.FirstOrDefault(p => p.ColorId == obj.Id) == null)
                //{
                ListMaterialsitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
                //}
            }
            ViewBag.productId = productId;
            ViewBag.ListSizeitems = ListSizeitems;
            ViewBag.ListColorsitems = ListColorsitems;
            ViewBag.ListMaterialsitems = ListMaterialsitems;
            return View();
        }
        private async Task<bool> ProductExists(Guid productId, Guid sizeId, Guid colorId, Guid materialId)
        {
            var a = await _productDetailService.Gets();
            return a.Any(pd =>
                  pd.ProductId == productId &&
                  pd.SizeId == sizeId &&
                  pd.ColorId == colorId &&
                  pd.MaterialId == materialId);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProductDetail(ProductDetail productDetail)
        {
            List<SelectListItem> ListSizeitems = new List<SelectListItem>();
            List<SelectListItem> ListColorsitems = new List<SelectListItem>();
            List<SelectListItem> ListMaterialsitems = new List<SelectListItem>();
            foreach (var obj in ((await _sizeService.Gets()).OrderBy(c => c.Name)))
            {
                ListSizeitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString(),

                });
            }
            foreach (var obj in (await _colorsService.Gets()).OrderBy(c => c.ColorName))
            {

                ListColorsitems.Add(new SelectListItem()
                {
                    Text = obj.ColorName,
                    Value = obj.Id.ToString(),
                });
            }
            foreach (var obj in (await _materialsService.Gets()).OrderBy(c => c.Name))
            {
                ListMaterialsitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListSizeitems = ListSizeitems;
            ViewBag.ListColorsitems = ListColorsitems;
            ViewBag.ListMaterialsitems = ListMaterialsitems;
            if (ModelState.IsValid)
            {
                if (
        productDetail.SizeId == Guid.Empty ||
        productDetail.ColorId == Guid.Empty ||
        productDetail.MaterialId == Guid.Empty || productDetail.PriceSale == null || productDetail.PriceSale < 0 || productDetail.Quantity <0 || productDetail.Quantity == null)
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ các trường.";
                    return RedirectToAction("CreateProductDetail", null, new { @productId = productDetail.ProductId });
                }
                bool exists = await ProductExists(productDetail.ProductId, productDetail.SizeId, productDetail.ColorId, productDetail.MaterialId);

                if (exists)
                {
                    TempData["ErrorMessage"] = "Sản phẩm với kích cỡ, màu sắc và chất liệu này đã tồn tại.";
                    return RedirectToAction("CreateProductDetail", null, new { @productId = productDetail.ProductId });
                }
                var product = await _productService.GetById(productDetail.ProductId);
                var size = await _sizeService.GetById(productDetail.SizeId);
                var colors = await _colorsService.GetById(productDetail.ColorId);
                var materials = await _materialsService.GetById(productDetail.MaterialId);
                productDetail.CreateAt = DateTime.Now;
                productDetail.SKU = product.ProductCode + "_" + size.Name.Trim().Replace(" ", "_").ToUpper() + colors.ColorName.Trim();
                productDetail.Status = productDetail.Status;
                var result = await _productDetailService.Add(productDetail);
                if (result)
                {
                    return RedirectToAction("productdetail", null, new { @productId = productDetail.ProductId });
                }
            }
            TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ các trường";
            return RedirectToAction("CreateProductDetail", null, new { @productId = productDetail.ProductId });
        }
        [HttpPost("Createsize")]
        public async Task<IActionResult> Createsizeinprdetail(string s, string idProduct)
        {
            var size = await _sizeService.GetByName(s);
            if (size != null)
            {
                TempData["ErrorMessage"] = "Kích cỡ đã có";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
            }
            else if (s == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
            }
            else
            {
                var sizes = new Size();
                sizes.Name = s;
                var result = await _sizeService.Add(sizes);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
                }
            }
            return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        }

        [HttpPost("CreateMaterialinProductDetail")]
        public async Task<IActionResult> CreateMaterialInProductDetail(string nameMaterial, string idProduct)
        {
            var size = await _materialsService.Gets();
            if (size.Any(c => c.Name == nameMaterial))
            {
                TempData["ErrorMessage"] = "Chất Liệu đã có";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });

            }
            else if (nameMaterial == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });

            }
            else
            {
                var ss = new Material();
                ss.Name = nameMaterial;
                var result = await _materialsService.Add(ss);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });

                }
            }
            return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });

        }

        [HttpPost("CreateinProductDetail")]
        public async Task<ActionResult> CreatecolorinPrDt(string colorName, string colorCode, string idProduct)
        {

            var color = await _colorsService.Gets();
            if (colorName == null)
            {
                TempData["ErrorMessage"] = "Bạn cần nhập tên màu";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
            }
            if (colorCode == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
            }
            if (color.Any(c => c.ColorName != null && c.ColorName.Trim().ToLower() == colorName.Trim().ToLower()))
            {
                TempData["ErrorMessage"] = "Màu đã có vui lòng thêm màu khác";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
            }
            else if (IsValidHexColor(colorCode.Trim()) == false)
            {
                TempData["ErrorMessage"] = "Mã màu bạn thêm không đúng định dạng";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
            }
            else if (color.Any(c => c.ColorCode != null && c.ColorCode.Trim().ToLower() == colorCode.Trim().ToLower()))
            {
                TempData["ErrorMessage"] = "Mã Màu đã có vui lòng thêm màu khác";
                return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
            }
            else
            {
                var colornew = new Colors();
                colornew.Id = Guid.NewGuid();
                colornew.ColorName = colorName.Trim();
                colornew.ColorCode = colorCode.Trim();
                TempData["colorid"] = colornew.Id;

                var result = await _colorsService.Add(colornew);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
                }
            }
            return RedirectToAction("CreateProductDetail", "Products", new { area = "Admin", productId = idProduct });
        }
        private bool IsValidHexColor(string? colorCode)
        {
            string pattern = @"^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$";
            return Regex.IsMatch(colorCode, pattern);
        }

        public async Task<bool> ProductExists(Guid productId, Guid sizeId, Guid colorId, Guid materialId, Guid excludeId = default)
        {
            var a = await _productDetailService.Gets();
            return a.Any(pd =>
                    pd.ProductId == productId &&
                    pd.SizeId == sizeId &&
                    pd.ColorId == colorId &&
                    pd.MaterialId == materialId &&
                    pd.Id != excludeId);
        }


        public async Task<IActionResult> EditProductDetail(Guid productDetailId)
        {
            var productDetail = await _productDetailService.GetById(productDetailId);
            var productDetails = await _productDetailService.GetByProductId(productDetail.ProductId);

            List<SelectListItem> ListSizeitems = new List<SelectListItem>();
            foreach (var obj in (await _sizeService.Gets()).OrderBy(c => c.Name))
            {
                //if (productDetails.FirstOrDefault(p => p.SizeId == obj.Id) == null || obj.Id == productDetail.SizeId)
                //{
                ListSizeitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString(),
                    Selected = obj.Id == productDetail.SizeId
                });
                //}
            }

            ViewBag.ListSizeitems = ListSizeitems;

            List<SelectListItem> ListColorsitems = new List<SelectListItem>();
            foreach (var obj in (await _colorsService.Gets()).OrderBy(c => c.ColorName))
            {
                //if (productDetails.FirstOrDefault(p => p.SizeId == obj.Id) == null || obj.Id == productDetail.SizeId)
                //{
                ListColorsitems.Add(new SelectListItem()
                {
                    Text = obj.ColorName,
                    Value = obj.Id.ToString(),
                    Selected = obj.Id == productDetail.ColorId
                });
                //}
            }
            ViewBag.ListColorsitems = ListColorsitems;

            List<SelectListItem> ListMaterialsitems = new List<SelectListItem>();

            foreach (var obj in (await _materialsService.Gets()).OrderBy(c => c.Name))
            {
                //if (productDetails.FirstOrDefault(p => p.ColorId == obj.Id) == null)
                //{
                ListMaterialsitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString(),
                    Selected = obj.Id == productDetail.MaterialId

                });
                //}
            }
            ViewBag.ListMaterialsitems = ListMaterialsitems;

            ViewBag.productId = productDetail.ProductId;

            return View(productDetail);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductDetail(ProductDetail productDetail)
        {
            List<SelectListItem> ListSizeitems = new List<SelectListItem>();
            foreach (var obj in ((await _sizeService.Gets()).OrderBy(c => c.Name)))

            {
                ListSizeitems.Add(new SelectListItem()
                {
                    Text = obj.Name,
                    Value = obj.Id.ToString()
                });
            }
            ViewBag.ListSizeitems = ListSizeitems;
            if (ModelState.IsValid)
            {
                bool exists = await ProductExists(productDetail.ProductId, productDetail.SizeId, productDetail.ColorId, productDetail.MaterialId, productDetail.Id);

                if (exists)
                {
                    ModelState.AddModelError("", "Sản phẩm với kích cỡ, màu sắc và chất liệu này đã tồn tại.");
                    return View(productDetail);
                }

                if (
       productDetail.SizeId == Guid.Empty ||
       productDetail.ColorId == Guid.Empty ||
       productDetail.MaterialId == Guid.Empty || productDetail.PriceSale == null || productDetail.PriceSale < 0 || productDetail.Quantity < 0 || productDetail.Quantity == null)
                {
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ các trường.");
                    return RedirectToAction("CreateProductDetail", null, new { @productId = productDetail.ProductId });
                }
                var product = await _productService.GetById(productDetail.ProductId);
                var size = await _sizeService.GetById(productDetail.SizeId);
                var material = await _materialsService.GetById(productDetail.MaterialId);
                productDetail.UpdateAt = DateTime.Now;
                productDetail.SKU = product.ProductCode + "-" + size.Name.Trim().Replace(" ", "_").ToUpper();
                productDetail.Status = productDetail.Status;
                var result = await _productDetailService.Update(productDetail);
                if (result)
                {
                    return RedirectToAction("productdetail", null, new { @productId = productDetail.ProductId });
                }
            }
            ModelState.AddModelError("", "Vui lòng nhập đầy đủ các trường");
            return RedirectToAction("EditProductDetail", null, new { @productId = productDetail.Id });
        }

        public async Task<IActionResult> DeleteProductDetail(Guid Id, Guid productId)
        {
            var result = await _productDetailService.Delete(Id);
            return RedirectToAction("ProductDetail", new { productId = productId });
        }
        public async Task<IActionResult> DownloadQrCode(Guid productDetailId)
        {
            var writer = new ZXing.QrCode.QRCodeWriter();
            var resultBit = writer.encode(productDetailId.ToString(), BarcodeFormat.QR_CODE, 100, 100);
            var matrix = resultBit;
            int scale = 2;
            var result = new Bitmap(matrix.Width * scale, matrix.Height * scale);
            for (int i = 0; i < matrix.Height; i++)
            {
                for (int j = 0; j < matrix.Width; j++)
                {
                    Color pixel = matrix[i, j] ? Color.Black : Color.White;
                    for (int n = 0; n < scale; n++)
                    {
                        for (int m = 0; m < scale; m++)
                        {
                            result.SetPixel(i * scale + n, j * scale + m, pixel);
                        }
                    }
                }
            }
            string webRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(webRootPath, "images", "qrcode", $"QRCode{productDetailId}.png");
            result.Save(imagePath);
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/png", $"QRCode{productDetailId}.png"); // Trả về hình ảnh dưới dạng nội dung của tệp
        }
    }
}
