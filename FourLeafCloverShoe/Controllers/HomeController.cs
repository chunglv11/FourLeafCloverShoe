using FourLeafCloverShoe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.WebSockets;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace FourLeafCloverShoe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
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