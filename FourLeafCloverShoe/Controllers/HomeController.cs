using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Models;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.WebSockets;
using X.PagedList;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace FourLeafCloverShoe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductService _productService;
        private readonly IProductDetailService _productDetailService;
        private readonly ISizeService _sizeService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _orderService;
        private readonly IRateService _rateService;
        private readonly UserManager<User> _userManager;
        private readonly IColorsService _colorsService;
        private readonly IPostService _ipostService;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IProductService productService, IOrderItemService orderItemService, IBrandService brandService, ICategoryService categoryService, IProductDetailService productDetailService, ISizeService sizeService, IRateService rateService, IOrderService orderService, UserManager<User> userManager,
            IColorsService colorsService, IPostService postService)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _productDetailService = productDetailService;
            _sizeService = sizeService;
            _categoryService = categoryService;
            _brandService = brandService;
            _orderItemService = orderItemService;
            _rateService = rateService;
            _orderService = orderService;
            _userManager = userManager;
            _colorsService = colorsService;
            _ipostService = postService;
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
        private async Task<List<Product>> GetTopSellingProducts()
        {
            // Lấy danh sách sản phẩm từ dịch vụ sản phẩm
            var products = await _productService.Gets();

            // Sắp xếp và lấy top sản phẩm bán chạy nhất
            var topSellingProducts = products
                .OrderByDescending(p => p.ProductDetails.Sum(pd => pd.OrderItems.Sum(oi => oi.Quantity)))
                // Lấy 10 sản phẩm bán chạy nhất
                .ToList();

            return topSellingProducts;
        }
        public async Task<IActionResult> Index(int? page, string searchString, string sortSelect, string[] size_group, string[] brand_group, string[] category_group, string price_range)
        {
            if (page == null)
                page = 1;
            int pageSize = 24;
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

            var topSellingProducts =  _productService.GetBestSellingProducts(10);

            ViewBag.TopSellingProducts = topSellingProducts.Products;
            var latestPosts = await _ipostService.GetLatestPosts(5);

            // Truyền danh sách bài viết vào ViewBag hoặc ViewModel
            ViewBag.LatestPosts = latestPosts;
            
            lstProduct = await Filter(searchString, sortSelect, size_group, brand_group, category_group, price_range, lstProduct); // lọc theo size, brand, category, range price
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Icon()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Icon(IFormCollection formCollection)
        {
            var writer = new ZXing.QrCode.QRCodeWriter();
            var resultBit = writer.encode(formCollection["QRCodeString"], BarcodeFormat.QR_CODE, 200, 200);
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
            result.Save(webRootPath + "\\images\\QRcodeNew.png");
            ViewBag.URL = "\\images\\QRcodeNew.png";
            return View();
        }
        public IActionResult ReadQRCode()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var path = webRootPath + "\\images\\QRcodeNew.png";
            var reader = new BarcodeReaderGeneric();
            Bitmap image = (Bitmap)Image.FromFile(path);
            var source = new BitmapLuminanceSource(image);

            // Use source with BarcodeReader
            var result = reader.Decode(source);
            Console.WriteLine(result);
            return View();
        }
        // C# (ASP.NET 6)
        [HttpPost]
        public IActionResult DecodeQRCode(string imageData)
        {
            // Chuyển đổi chuỗi base64 thành Bitmap
            var bytes = Convert.FromBase64String(imageData.Split(',')[1]);
            using var ms = new MemoryStream(bytes);
            var bitmap = new Bitmap(ms);

            // Giải mã mã QR
            var reader = new BarcodeReader();
            var result = reader.Decode(new BitmapLuminanceSource(bitmap));

            if (result != null)
            {
                // Mã QR được giải mã thành công
                return Ok(result.Text);
            }
            else
            {
                // Không tìm thấy mã QR trong hình ảnh
                return NotFound();
            }
        }


    }
}