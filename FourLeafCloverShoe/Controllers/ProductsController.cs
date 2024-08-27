using FourLeafCloverShoe.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR;
using FourLeafCloverShoe.Helper;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using ZXing;
using System.Diagnostics.Metrics;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using X.PagedList;
using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace FourLeafCloverShoe.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductDetailService _productDetailService;
        private readonly ISizeService _sizeService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _orderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRateService _rateService;
        private readonly UserManager<User> _userManager;
        private readonly IColorsService _colorsService;
        private readonly IMaterialService _materialService;

        public ProductsController(IProductService productService, IProductDetailService productDetailService, ISizeService sizeService, ICategoryService categoryService, IBrandService brandService, IOrderItemService orderItemService, IOrderService orderService, IWebHostEnvironment webHostEnvironment, IRateService rateService, UserManager<User> userManager, IColorsService colorsService, IMaterialService materialService)
        {
            _productService = productService;
            _productDetailService = productDetailService;
            _sizeService = sizeService;
            _categoryService = categoryService;
            _brandService = brandService;
            _orderItemService = orderItemService;
            _orderService = orderService;
            _webHostEnvironment = webHostEnvironment;
            _rateService = rateService;
            _userManager = userManager;
            _colorsService = colorsService;
            _materialService = materialService;
        }

        public async Task<List<Product>> Filter(string searchString, string sortSelect, string[] size_group, string[] brand_group, string[] category_group, string price_range, List<Product> lstProduct)
        {
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                lstProduct = lstProduct.FindAll(c => c.ProductName.ToLower().Contains(searchString.ToLower()));
            }
            if (size_group.Length > 0)
            {
                lstProduct = lstProduct.FindAll(c => c.ProductDetails.Any(p => size_group.Contains(p.Size.Name)));
            }
            if (brand_group.Length > 0)
            {
                lstProduct = lstProduct.FindAll(c => c.ProductDetails.Any(p => brand_group.Contains(c.Brands.Name)));
            }
            if (category_group.Length > 0)
            {
                lstProduct = lstProduct.FindAll(c => c.ProductDetails.Any(p => category_group.Contains(c.Categories.Name)));
            }
            if (!string.IsNullOrWhiteSpace(price_range))
            {
                switch (price_range)
                {
                    case "duoi100":
                        lstProduct = lstProduct.FindAll(c => c.ProductDetails.Any(p => p.PriceSale <= 100000));
                        break;

                    case "100-200":
                        lstProduct = lstProduct.FindAll(c => c.ProductDetails.Any(p => p.PriceSale >= 100000 && p.PriceSale <= 200000));
                        break;

                    case "200-300":
                        lstProduct = lstProduct.FindAll(c => c.ProductDetails.Any(p => p.PriceSale >= 200000 && p.PriceSale <= 300000));
                        break;

                    case "tren300":
                        lstProduct = lstProduct.FindAll(c => c.ProductDetails.Any(p => p.PriceSale >= 300000));
                        break;

                    default:
                        break;
                }
            }
            if (!String.IsNullOrWhiteSpace(sortSelect))
            {
                switch (sortSelect)
                {
                    case "PRICEASC":
                        lstProduct = lstProduct.OrderBy(c => c.ProductDetails.Min(p => p.PriceSale)).ToList();
                        break;

                    case "PRICEDESC":
                        lstProduct = lstProduct.OrderByDescending(c => c.ProductDetails.Max(p => p.PriceSale)).ToList();
                        break;

                    case "NAMEAZ":
                        lstProduct = lstProduct.OrderBy(c => c.ProductName).ToList();
                        break;

                    case "NAMEZA":
                        lstProduct = lstProduct.OrderByDescending(c => c.ProductName).ToList();
                        break;

                    case "DATENEW":
                        lstProduct = lstProduct.OrderByDescending(c => c.CreateAt).ToList();
                        break;

                    case "DATEOLD":
                        lstProduct = lstProduct.OrderBy(c => c.CreateAt).ToList();
                        break;

                    case "BESTSALE":
                        var lstOrderItem = await _orderItemService.Gets();
                        lstProduct = lstProduct.OrderByDescending(c => lstOrderItem.Where(p => p.ProductDetails.ProductId == c.Id).Sum(p => p.Quantity)).ToList();
                        break;

                    default:
                        break;
                }
            }
            return lstProduct;
        }

        public async Task<IActionResult> Index(int? page, string searchString, string sortSelect, string[] size_group, string[] brand_group, string[] category_group, string price_range)
        {
            if (page == null)
                page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var lstProduct = (await _productService.Gets()).Where(c => c.Status == true && c.ProductDetails.Where(p => p.Status == 1).Count() > 0).ToList();
            var Size = new List<string>();
            foreach (var size in await _sizeService.Gets())
            {
                Size.Add(size.Name);
            }
            ViewBag.Size = Size;
            ViewBag.SelectedSize = size_group.ToList(); // Lưu trữ các size đã chọn

            var Brand = new List<string>();
            foreach (var brand in await _brandService.Gets())
            {
                Brand.Add(brand.Name);
            }
            ViewBag.Brand = Brand;
            ViewBag.SelectedBrand = brand_group.ToList(); // Lưu trữ các brand đã chọn

            var Category = new List<string>();
            foreach (var cate in await _categoryService.Gets())
            {
                Category.Add(cate.Name);
            }
            ViewBag.Category = Category;
            var sortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "PRICEASC", Text = "Theo giá: từ thấp đến cao" },
                new SelectListItem { Value = "PRICEDESC", Text = "Theo giá: từ cao đến thấp" },
                new SelectListItem { Value = "NAMEAZ", Text = "Theo tên từ: A-Z" },
                new SelectListItem { Value = "NAMEZA", Text = "Theo tên từ: Z-A" },
                new SelectListItem { Value = "DATENEW", Text = "Sản phẩm mới nhất" },
                new SelectListItem { Value = "DATEOLD", Text = "Sản phẩm cũ" },
                new SelectListItem { Value = "BESTSALE", Text = "Sản phẩm bán chạy" }
                // Thêm các tùy chọn khác tại đây
            };
            ViewBag.SortSelect = new SelectList(sortOptions, "Value", "Text", sortSelect); // Tạo SelectList với giá trị đã chọn
            ViewBag.SelectedCategory = category_group.ToList(); // Lưu trữ các category đã chọn
            ViewBag.PriceRange = price_range;
            ViewBag.SearchString = searchString;

            lstProduct = await Filter(searchString, sortSelect, size_group, brand_group, category_group, price_range, lstProduct); // lọc theo size, brand, category, range price
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> ProductDetail(Guid productId)
        {
            var product = await _productService.GetById(productId);
            var lstProductDetail = await _productDetailService.GetByProductId(product.Id);

            var sizes = await _sizeService.Gets();
            var colors = await _colorsService.Gets();

            var lstSize = sizes.Where(size => lstProductDetail.Any(detail => detail.SizeId == size.Id)).OrderBy(c => c.Name).ToList();
            var lstColors = colors.Where(color => lstProductDetail.Any(detail => detail.ColorId == color.Id)).OrderBy(c => c.ColorName).ToList();


            var priceMin = lstProductDetail.Where(c => c.Status == 1).Min(c => c.PriceSale);
            var priceMax = lstProductDetail.Where(c => c.Status == 1).Max(c => c.PriceSale);
            var availibleQuantity = lstProductDetail.Where(c => c.Status == 1).Sum(c => c.Quantity);
            ViewBag.lstSize = lstSize;
            ViewBag.lstColors = lstColors;
            ViewBag.priceMin = priceMin;
            ViewBag.priceMax = priceMax;
            ViewBag.availibleQuantity = availibleQuantity;
            //lấy sao đánh giá để tính tổng dựa theo id product
            var productServiceGets = await _productService.Gets();
            var productDetailServiceGets = await _productDetailService.Gets();
            var orderItemServiceGets = await _orderItemService.Gets();
            var rateServiceGets = await _rateService.Gets();
            List<RateViewModel> lstRate = (from sp in productServiceGets
                                           join ctsp in productDetailServiceGets on sp.Id equals ctsp.ProductId
                                           join cthd in orderItemServiceGets on ctsp.Id equals cthd.ProductDetailId
                                           join dg in rateServiceGets on cthd.Id equals dg.OrderItemId
                                           where sp.Id == productId && dg.Status == 1
                                           select new RateViewModel
                                           {
                                               ID = dg.Id,
                                               IDPro = sp.Id,
                                               Rating = dg.Rating,
                                               Status = dg.Status,
                                           }).ToList();
            ViewBag.lstRate = lstRate;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> getdatabysizeid(string sizeId, string productId, string colorId)
        {
            var product = await _productService.GetById(Guid.Parse(productId));
            var lstProductDetail = await _productDetailService.GetByProductId(product.Id);
            var productDetail = lstProductDetail.FirstOrDefault(c => c.SizeId == Guid.Parse(sizeId) && c.ColorId == Guid.Parse(colorId));
            var status = productDetail.Status;

            if (productDetail.Products.Status == true && status == 1)
            {
                status = 1;
            }
            else
            {
                status = 0;
            }


            return Json(new { productDetailId = productDetail.Id, priceSale = productDetail.PriceSale, quantity = productDetail.Quantity, status = status, imgQrCode = GenerateQRCodeAsync(productDetail.Id) });
        }
        [HttpPost]
        public async Task<IActionResult> getavailableoptions(string productId, string? sizeId = null, string? colorId = null)
        {
            // Kiểm tra tính hợp lệ của productId
            if (!Guid.TryParse(productId, out Guid parsedProductId))
            {
                return BadRequest("Mã sản phẩm không hợp lệ");
            }

            // Lấy thông tin sản phẩm
            var product = await _productService.GetById(parsedProductId);
            if (product == null || product.Status != true)
            {
                return NotFound("Không tìm thấy sản phẩm hoặc sản phẩm không hoạt động");
            }


            // Lấy danh sách chi tiết sản phẩm
            var productDetails = (await _productDetailService.GetByProductId(product.Id));

            // Lọc dựa trên size và color đã chọn (nếu có)
            Guid? parsedSizeId = null;
            Guid? parsedColorId = null;
            if (Guid.TryParse(sizeId, out Guid parsedSizeGuid))
            {
                parsedSizeId = parsedSizeGuid;
                productDetails = productDetails.Where(pd => pd.SizeId == parsedSizeId).ToList();
            }
            if (Guid.TryParse(colorId, out Guid parsedColorGuid))
            {
                parsedColorId = parsedColorGuid;
                productDetails = productDetails.Where(pd => pd.ColorId == parsedColorId).ToList();
            }

            // Danh sách các size và color khả dụng dựa trên bộ lọc
            var availableSizes = productDetails.Select(pd => pd.SizeId).Distinct().ToList();
            var availableColors = productDetails.Select(pd => pd.ColorId).Distinct().ToList();
            return Json(new
            {
                sizes = availableSizes,
                colors = availableColors
            });


        }
        public string GenerateQRCodeAsync(Guid productDetailId)
        {
            var writer = new ZXing.QrCode.QRCodeWriter();
            var resultBit = writer.encode(productDetailId.ToString(), BarcodeFormat.QR_CODE, 100, 100);
            var matrix = resultBit;
            int scale = 1;
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
            string qrCodeFolderPath = Path.Combine(webRootPath, "images", "qrcode");
            // Lấy danh sách tất cả các tệp trong thư mục QRCode
            string[] files = Directory.GetFiles(qrCodeFolderPath);

            // Xoá từng tệp ảnh trong thư mục
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
            result.Save(webRootPath + $"\\images\\qrcode\\QRCode{productDetailId}.png");
            var imgQrCode = $"\\images\\qrcode\\QRCode{productDetailId}.png";
            return imgQrCode;
        }
        [HttpGet]
        public async Task<IActionResult> ReviewProducts(Guid idCTHD)
        {
            //var revi = (await _orderItemService.Gets()).FirstOrDefault(c => c.Id == idCTHD);
            return Redirect($"/Identity/Account/Manage/ReviewProducts?idCTHD={idCTHD}");//Dcm mãi ms sang view blazor dc cú vl
        }

        [HttpPost]
        public async Task<IActionResult> RateProducts(Guid id, Guid idCTHD, float rating, string? danhGia, Guid idHD, List<IFormFile> uploadedImages)
        {
            var imageUrls = new List<string>();

            //BÚ C#4
            foreach (var image in uploadedImages)
            {
                if (image != null && image.Length > 0)
                {
                    // Định nghĩa đường dẫn và tên tệp ảnh
                    var filePath = Path.Combine("wwwroot/images/uploads", image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào danh sách
                    var imageUrl = $"/images/uploads/{image.FileName}";
                    imageUrls.Add(imageUrl);
                }
            }

            // Tạo chuỗi URL từ danh sách
            var imageUrlsString = string.Join(";", imageUrls);
            if (danhGia != null && danhGia.Length > 200)
            {
                //ModelState.AddModelError("danhGia", "Đánh giá không được vượt quá 200 ký tự.");
                TempData["ErrorMessage"] = "Đánh giá không được vượt quá 200 ký tự.";
            }
            if (rating == 0)
            {
                TempData["ErrorMessage"] = "Bạn có thể cho shop 5* được không :(";
            }
            else
            {
                var ratePr = await _rateService.UpdateDanhGia(id, idCTHD, rating, danhGia, imageUrlsString);
                if (ratePr)
                {
                    return Redirect($"/Identity/Account/Manage/orderdetail?orderId={idHD}");// Chuyển sang trang đơn chi tiết
                }
            }

            return Redirect($"/Identity/Account/Manage/ReviewProducts?idCTHD={idCTHD}");
        }

        [HttpGet]
        public async Task<IActionResult> ShowRateByIdProduct(Guid IdPro)
        {
            var productServiceGets = await _productService.Gets();
            var productDetailServiceGets = await _productDetailService.Gets();
            var orderItemServiceGets = await _orderItemService.Gets();
            var rateServiceGets = await _rateService.Gets();
            var orderServiceGets = await _orderService.Gets();
            var sizeServiceGets = await _sizeService.Gets();
            var colorServiceGets = await _colorsService.Gets();
            var mateServiceGets = await _materialService.Gets();
            List<RateViewModel> lstRate = (from sp in productServiceGets
                                           join ctsp in productDetailServiceGets on sp.Id equals ctsp.ProductId
                                           join cthd in orderItemServiceGets on ctsp.Id equals cthd.ProductDetailId
                                           join dg in rateServiceGets on cthd.Id equals dg.OrderItemId
                                           join hd in orderServiceGets on cthd.OrderId equals hd.Id
                                           join kc in sizeServiceGets on ctsp.SizeId equals kc.Id
                                           join ms in colorServiceGets on ctsp.ColorId equals ms.Id
                                           join cl in mateServiceGets on ctsp.MaterialId equals cl.Id
                                           where sp.Id == IdPro && dg.Status == 1
                                           select new RateViewModel
                                           {
                                               ID = dg.Id,
                                               IDPro = sp.Id,
                                               Rating = dg.Rating,
                                               Contents = dg.Contents,
                                               Status = dg.Status,
                                               ImageUrl = dg.ImageUrl,
                                               CreateDate = dg.CreateDate.GetValueOrDefault().ToString("dd/MM/yyyyy HH:mm:ss"),
                                               TenKH = _userManager.Users.FirstOrDefault(c => c.Id == hd.UserId).FullName,
                                               AnhKh =  _userManager.Users.FirstOrDefault(c => c.Id == hd.UserId).ProfilePicture != null ? Convert.ToBase64String(_userManager.Users.FirstOrDefault(c => c.Id == hd.UserId).ProfilePicture) : "",
                                               Size = kc.Name,
                                               Color = ms.ColorName,
                                               Material = cl.Name
                                           }).ToList();
            return Json(lstRate);
        }
    }
}