using FourLeafCloverShoe.Controllers;
using FourLeafCloverShoe.Helper;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Libraries;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Text;
using ZXing;
using ZXing.Windows.Compatibility;
using static System.Net.Mime.MediaTypeNames;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanTaiQuayController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IProductDetailService _productDetailService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IUserVoucherService _userVoucherService;
        private readonly IVoucherService _voucherService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICartService _cartService;
        private readonly IRanksService _ranksService;
        private readonly IEmailSender _emailSender;
        private readonly IHubContext<Hubs> _hubContext;

        public BanTaiQuayController(UserManager<User> userManager,
            IProductDetailService productDetailService,
            IOrderService orderService,
             RoleManager<IdentityRole> roleManager,
            IOrderItemService orderItemService,
            IUserVoucherService userVoucherService,
            IVoucherService voucherService,
             IEmailSender emailSender,
             IHubContext<Hubs> hubContext,
             IRanksService ranksService,
            ICartService cartService
            )
        {
            _userManager = userManager;
            _productDetailService = productDetailService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _userVoucherService = userVoucherService;
            _voucherService = voucherService;
            _roleManager = roleManager;
            _cartService = cartService;
            _ranksService = ranksService;
            _emailSender = emailSender;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetPhoneNumbersAsync()
        {
            var lstUser = await _userManager.Users.ToListAsync();
            // Lấy dữ liệu số điện thoại từ nguồn nào đó
            var phoneNumbers = new List<SelectListItem>();
            foreach (var item in lstUser)
            {
                phoneNumbers.Add(new SelectListItem() { Text = item.PhoneNumber, Value = item.Id });
            }


            // Trả về dữ liệu dưới dạng JSON
            return Json(phoneNumbers);
        }
        public async Task<IActionResult> GetProductDetailsAsync()
        {
            var productDetails = await _productDetailService.Gets();
            var listProductDetail = new List<ProductDeailViewModel>();
            foreach (var item in productDetails)
            {
                bool status = false;
                if (item.Status == 1 && item.Products.Status == true)
                {
                    status = true;
                }
                listProductDetail.Add(new ProductDeailViewModel() { Id = item.Id, Quantity = item.Quantity, ImageUrl = item.Products.ProductImages.First().ImageUrl, ProductName = item.Products.ProductName, SizeName = item.Size.Name, Price = item.PriceSale, Status = status });
            }
            // Trả về dữ liệu dưới dạng JSON
            return Json(listProductDetail);
        }
        [HttpPost]
        public async Task<IActionResult> GetOrderItemsAsync(Guid orderId)
        {
            var lstOrderItem = (await _orderItemService.Gets()).Where(c => c.OrderId == orderId);
            // Lấy dữ liệu số điện thoại từ nguồn nào đó

            var listProductDetail = new List<ProductDeailViewModel>();
            foreach (var item in lstOrderItem)
            {
                listProductDetail.Add(new ProductDeailViewModel() { Id = (Guid)item.ProductDetailId, Quantity = item.Quantity, ImageUrl = item.ProductDetails.Products.ProductImages.First().ImageUrl, ProductName = item.ProductDetails.Products.ProductName, SizeName = item.ProductDetails.Size.Name, Price = item.Price, Total = item.Price * item.Quantity });
            }
            // Trả về dữ liệu dưới dạng JSON
            return Json(listProductDetail);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToOrder(Guid orderId, string userId)
        {
            if (orderId == null || orderId == new Guid())
            {
                return Json(new { message = "Bạn chưa tạo đơn hàng mới!", isSuccess = false });
            }
            var order = await _orderService.GetById(orderId);
            if (order.OrderStatus==-1)
            {
                var user = await _userManager.FindByIdAsync(userId);
                order.UserId = userId;
                order.RecipientPhone = user.PhoneNumber;
                order.RecipientName = user.FullName;
                order.RecipientAddress = "Mua hàng tại quầy";
                var result = await _orderService.Update(order);
                return Json(new { message = "Thêm khách hàng thành công!", isSuccess = result });
            }
            if (order.OrderStatus == 9)
            {
                return Json(new { message = "Đơn hàng đã được thanh toán, không thể chỉnh sửa!", isSuccess = false });
            }
            if (order.OrderStatus == 13)
            {
                return Json(new { message = "Đơn hàng đã được huỷ, không thể chỉnh sửa!", isSuccess = false });
            }

            return Json(new { message = "Lỗi không xác định!", isSuccess = false });

        }
        [HttpPost]
        public async Task<JsonResult> ApplyVoucher(Guid voucherId)
        {
            if (voucherId != new Guid())
            {
                var voucherSelected = (await _voucherService.GetById(voucherId));

                return Json(new { Id = voucherSelected.Id, voucherType = voucherSelected.VoucherType, voucherValue = voucherSelected.VoucherValue, maxDiscount = voucherSelected.MaximumOrderValue, isSuccess = true });
            }
            return Json(new { isSuccess = false });
        }
        public async Task<IActionResult> loadDataUserOrder(Guid orderId)
        {
            if (orderId != new Guid())
            {
                var order = await _orderService.GetById(orderId);
                var orderItems = (await _orderItemService.Gets()).Where(c => c.OrderId == orderId);
                var total = orderItems.Sum(c => c.Quantity * c.Price);
                order.TotalAmout = total; //  cái này còn phải trừ coin, voucher
                await _orderService.Update(order);
                var userVouchers = (await _userVoucherService.Gets()).Where(c => c.UserId == order.UserId && c.Status == 1);
                var productQuantity = orderItems.Sum(c => c.Quantity);
                var lstVoucher = new List<SelectListItem>();
                if (order.UserId != null)
                {
                    var user = await _userManager.FindByIdAsync(order.UserId);
                    var vouchers = (await _voucherService.Gets()).Where(c => userVouchers.Any(p => c.Id == p.VoucherId) && c.Quantity > 0 && c.Status == 1 && c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now);
                    foreach (var obj in vouchers.Where(c => c.MinimumOrderValue <= total))
                    {
                        var giamToiDa = obj.MaximumOrderValue;
                        var giaTriVoucher = obj.VoucherValue?.ToString("0.##");
                        string text = "";
                        if (obj.VoucherType == 1)
                        {
                            giaTriVoucher += " %";
                            text = $"Mã giảm {giaTriVoucher} tối đa {giamToiDa?.ToString("N0")} đ đơn từ {obj.MinimumOrderValue?.ToString("N0")}đ ";
                        }
                        else
                        {
                            giaTriVoucher = String.Format("N0", giaTriVoucher) + " đ";
                            text = $"Mã giảm {giamToiDa?.ToString("N0")} đ đơn từ {obj.MinimumOrderValue?.ToString("N0")}đ ";
                        }
                        lstVoucher.Add(new SelectListItem()
                        {
                            Text = text,
                            Value = obj.Id.ToString()
                        });
                    }

                    return Json(new { fullName = user.FullName, phoneNumber = user.PhoneNumber, coins = user.Coins, total = total, lstVoucher = lstVoucher, voucherValue = order.VoucherValue, productQuantity = productQuantity, orderStatus= order.OrderStatus });
                }
                return Json(new { fullName = "", phoneNumber = "", coins = 0, total = total, lstVoucher = lstVoucher, voucherValue = order.VoucherValue, productQuantity = productQuantity, orderStatus = order.OrderStatus });
            }
            return Json(new { message = "Order null!", isSuccess = false });
        }

        public async Task<IActionResult> GetPendingOrders()
        {
            var lstDonCho = (await _orderService.Gets()).Where(c => c.OrderStatus == -1); // lấy danh sách đơn chờ
            var lstDonChoHetHan = lstDonCho.Where(c => c.CreateDate.Value.Date != DateTime.Today.Date); // lấy danh sách đơn chờ hết hạn
            var lstSanPhamDonChoHetHan = (await _orderItemService.Gets()).Where(c => lstDonChoHetHan.Any(b => b.Id == c.OrderId)); // lấy danh sách orderitem trong đơn chờ hết hạn
            foreach (var item in lstSanPhamDonChoHetHan) // hoàn kho sản phẩm
            {
                var productDetail = await _productDetailService.GetById((Guid)item.ProductDetailId);
                productDetail.Quantity += item.Quantity;
                await _productDetailService.Update(productDetail);
            }
            await _orderItemService.DeleteMany(lstSanPhamDonChoHetHan.ToList()); // xoá sp hết hạn ở orderItem
            await _orderService.DeleteMany(lstDonChoHetHan.ToList()); // xoá đơn hết hạn
            var lstGuid = new List<Guid>();
            foreach (var item in (await _orderService.Gets()).Where(c => c.OrderStatus == -1)) // lưu trữ id đơn hàng
            {
                lstGuid.Add(item.Id);
            }
            return Json(lstGuid);
        }
        [HttpPost]
        public async Task RemovePendingOrder(Guid orderId)
        {
            var order = (await _orderService.Gets()).FirstOrDefault(c => c.Id == orderId);
            if (order != null)
            {
                if (order.OrderStatus == -1)
                {
                    var orderItems = (await _orderItemService.Gets()).Where(c => c.OrderId == orderId);


                    if (orderItems.Count() > 0)
                    {
                        var lstProductDetail = (await _productDetailService.Gets()).Where(c => orderItems.Any(p => p.ProductDetailId == c.Id)); // hoàn lại sp vào kho
                        foreach (var item in lstProductDetail)
                        {
                            item.Quantity += orderItems.FirstOrDefault(c => c.ProductDetailId == item.Id).Quantity;
                            await _productDetailService.UpdateMany(lstProductDetail.ToList());
                        }
                        await _orderItemService.DeleteMany(orderItems.ToList());
                    }
                    await _orderService.Delete(orderId);
                }
            }
        }

        public async Task<IActionResult> CreateNewOrderAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                TotalAmout = 0,
                OrderStatus = -1, // trạng thái đơn chờ
                OrderCode = OrderController.GenerateInvoiceCode("off"),
                StaffId = user.Id,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CoinsUsed = 0,
                VoucherValue = 0,
                ShippingFee = 0
            };
            var result = await _orderService.Add(order);
            if (result)
            {
                return Json(new { orderId = order.Id });
            }
            return Json(new { orderId = Guid.NewGuid() });

        }
        [HttpPost]
        public async Task<IActionResult> AddProductDetailToOrderAsync(Guid orderId, Guid productDetailId) // cần - số lượng trong db
        {

            if (orderId == null || orderId == new Guid())
            {
                return Json(new { isSuccess = false, message = "Bạn chưa tạo đơn hàng mới!" });
            }
            if (productDetailId == null || productDetailId == new Guid())
            {
                return Json(new { isSuccess = false, message = "Mã QR không hợp lệ!" });
            }

            var order = await _orderService.GetById(orderId);
            if (order.OrderStatus == 9)
            {
                return Json(new { message = "Đơn hàng đã được thanh toán, không thể chỉnh sửa!", isSuccess = false });
            }
            if (order.OrderStatus == 13)
            {
                return Json(new { message = "Đơn hàng đã được huỷ, không thể chỉnh sửa!", isSuccess = false });
            }

            var productDetail = await _productDetailService.GetById(productDetailId);
            if (order != null && productDetail != null)
            {
                if (productDetail.Status != 1 || productDetail.Products.Status != true)
                {

                    return Json(new { isSuccess = false, message = "Sản phẩm ngừng kinh doanh!" });
                }
                else if (productDetail.Quantity <= 0)
                {

                    return Json(new { isSuccess = false, message = "Sản phẩm không đủ số lượng trong kho!" });
                }
                else
                {
                    var lstOrderItems = (await _orderItemService.Gets()).Where(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
                    if (lstOrderItems.Count() == 0) // nếu orderItem không tồn tại thì thêm mới
                    {
                        var orderItem = new OrderItem()
                        {
                            Id = Guid.NewGuid(),
                            ProductDetailId = productDetail.Id,
                            OrderId = order.Id,
                            Quantity = 1,
                            Price = productDetail.PriceSale
                        };
                        var resultCreateOrderItem = await _orderItemService.Add(orderItem);
                        productDetail.Quantity -= 1;
                        await _productDetailService.Update(productDetail);
                        return Json(new { isSuccess = resultCreateOrderItem, message = "Cập nhật thành công" });
                    }
                    else // cộng dồn số lượng
                    {
                        var orderItem = (await _orderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
                        orderItem.Quantity += 1;
                        var resultCreateOrderItem = await _orderItemService.Update(orderItem);
                        productDetail.Quantity -= 1;
                        await _productDetailService.Update(productDetail);
                        return Json(new { isSuccess = resultCreateOrderItem, message = "Cập nhật thành công" });
                    }
                }
            }
            return Json(new { isSuccess = false, message = "Lỗi không xác định" });
        }
        public async Task<IActionResult> UpdateQuantityAsync(Guid productDetailId, Guid orderId, int newQuantity) // cần update số lượng trong db và check kho
        {
            var order = await _orderService.GetById(orderId);
            if (order.OrderStatus == 9)
            {
                return Json(new { message = "Đơn hàng đã được thanh toán, không thể chỉnh sửa!", isSuccess = false });
            }
            if (order.OrderStatus == 13)
            {
                return Json(new { message = "Đơn hàng đã được huỷ, không thể chỉnh sửa!", isSuccess = false });
            }

            var productDetail = await _productDetailService.GetById(productDetailId);
            var orderItem = (await _orderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
            if (productDetail.Quantity + orderItem.Quantity < newQuantity)
            {
                return Json(new { message = "Sản phẩm vượt quá giới hạn trong kho", isSuccess = false });
            }
            if (newQuantity <= 0)
            {
                return Json(new { message = "Sản phẩm tối thiểu là 1", isSuccess = false });
            }
            if (orderItem != null)
            {
                productDetail.Quantity = productDetail.Quantity + orderItem.Quantity - newQuantity;
                orderItem.Quantity = newQuantity;
                var result = await _orderItemService.Update(orderItem);
                if (result)
                {
                    await _productDetailService.Update(productDetail);
                    return Json(new { message = "Cập nhật thành công", isSuccess = true });
                }
            }
            return Json(new { message = "Lỗi không xác định", isSuccess = false });
        }
        public async Task<IActionResult> RemoveOrderItemAsync(Guid productDetailId, Guid orderId) // cần update lại số lượng trong db
        {
            var order = await _orderService.GetById(orderId);
            if (order.OrderStatus == 9)
            {
                return Json(new { message = "Đơn hàng đã được thanh toán, không thể chỉnh sửa!", isSuccess = false });
            }
            if (order.OrderStatus == 13)
            {
                return Json(new { message = "Đơn hàng đã được huỷ, không thể chỉnh sửa!", isSuccess = false });
            }

            var orderItem = (await _orderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
            if (orderItem != null)
            {
                var result = await _orderItemService.Delete(orderItem.Id);
                if (result)
                {
                    var productDetail = await _productDetailService.GetById(productDetailId);
                    productDetail.Quantity += orderItem.Quantity;
                    await _productDetailService.Update(productDetail);
                    return Json(new { message = "Cập nhật thành công", isSuccess = true });
                }
            }
            return Json(new { message = "Lỗi không xác định", isSuccess = false });
        }
        [HttpPost]
        public async Task<IActionResult> Register(string phoneNumber, string email, string fullName)
        {
            var password = GenerateRandomString();
            var usermodel = new User()
            {
                UserName = new MailAddress(email).User,
                Email = email,
                FullName = fullName,
                PhoneNumber = phoneNumber,
                Points = 0,
                Coins = 0,
                EmailConfirmed = true,
                RankId = Guid.Parse("2FA0118D-B530-421F-878E-CE4D54BFC6AB")
            };
            var result = await _userManager.CreateAsync(usermodel, password);
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                {
                    var roleResult = await _userManager.AddToRoleAsync(usermodel, role.Name);
                    if (roleResult.Succeeded)
                    {
                        var createCartResult = await _cartService.Add(new Cart() { Id = Guid.NewGuid(), UserId = usermodel.Id });

                        if (createCartResult)
                        {
                            // Send email confirmation email
                            await _emailSender.SendEmailAsync(email, "Đăng kí tài khoản thành công",
                                $"Mật khẩu của bạn tại Four Leaf Clover Store là : {password} .");
                            return Json(new { isSuccess = true, message = "Đăng kí tài khoản thành viên thành công!" });
                        }
                    }
                }
            }
            return NotFound();
        }
        public string GenerateRandomString()
        {
            var random = new Random();
            var lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
            var upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numberChars = "1234567890";
            var specialChars = "!@#$%^&*()";

            var stringChars = new char[8];
            stringChars[0] = lowerCaseChars[random.Next(lowerCaseChars.Length)];
            stringChars[1] = upperCaseChars[random.Next(upperCaseChars.Length)];
            stringChars[2] = numberChars[random.Next(numberChars.Length)];
            stringChars[3] = specialChars[random.Next(specialChars.Length)];

            var allChars = lowerCaseChars + upperCaseChars + numberChars + specialChars;
            for (int i = 4; i < stringChars.Length; i++)
            {
                stringChars[i] = allChars[random.Next(allChars.Length)];
            }

            // Trộn các ký tự trong mảng để tạo mật khẩu ngẫu nhiên
            return new string(stringChars.OrderBy(x => random.Next()).ToArray());
        }
        [HttpPost]
        public async Task<IActionResult> huyDon(Guid orderId)
        {
            var order = await _orderService.GetById(orderId);
            order.OrderStatus = 13;
            var orderItem = (await _orderItemService.Gets()).Where(c =>  c.OrderId == orderId);
            var lstProductDetail = await _productDetailService.Gets();
            var lstProductDetailOfOrderItems = lstProductDetail.Where(c=> orderItem.Any(p=>c.Id==p.ProductDetailId));
            if (orderItem.Count()<=0)
            {
                return Json(new { message = "Đơn hàng chưa có sản phẩm nào!", isSuccess = false });
            }
            foreach (var item in orderItem)
            {
                var productDetail = lstProductDetailOfOrderItems.FirstOrDefault(c => c.Id == item.ProductDetailId);
                productDetail.Quantity += item.Quantity;
            }
            var result1 = await _productDetailService.UpdateMany(lstProductDetailOfOrderItems.ToList());
            if (!result1)
            {
                return Json(new { message = "Lỗi cập nhật số lượng sản phẩm", isSuccess = false });

            }
            var result2 = await _orderService.Update(order);
            if (!result2)
            {
                return Json(new { message = "Lỗi cập nhật trạng thái đơn hàng", isSuccess = false });

            }
                return Json(new { message = "Huỷ đơn thành công", isSuccess = true });
        }
        [HttpPost]
        public async Task<IActionResult> CheckOutAsync(Guid orderId, Guid voucherId, decimal coinUsed, decimal voucherValue, string paymentType)
        {
            var order = await _orderService.GetById(orderId);
            var orderItem = (await _orderItemService.Gets()).Where(c =>  c.OrderId == orderId);
            if (orderItem.Count() <= 0)
            {
                return Json(new { message = "Đơn hàng chưa có sản phẩm nào!", isSuccess = false });
            }
            order.PaymentType = paymentType;
            order.UpdateDate = DateTime.Now;
            if (voucherId != new Guid())
            {
                order.VoucherId = voucherId;
                order.VoucherValue = voucherValue;
            }
            order.CoinsUsed = coinUsed;
            order.TotalAmout = order.TotalAmout - voucherValue - coinUsed;

            if (paymentType == "off" || order.TotalAmout == 0)
            {
                order.OrderStatus = 9;
                if (await _orderService.Update(order)) 
                {
                    var coinsPlus = (order.TotalAmout + voucherValue + coinUsed) / 100;
                    if (order.UserId!=null)//  cộng xu
                    {
                        var user = await _userManager.FindByIdAsync(order.UserId);
                        user.Coins += coinsPlus;
                        // còn phần rank
                        user.Points += (int)(order.TotalAmout + voucherValue + coinUsed);
                        var rank = (await _ranksService.Gets()).FirstOrDefault(c=>c.PointsMin <= user.Points&& c.PoinsMax>= user.Points);
                        if (user.RankId!= rank.Id)
                        {
                            user.RankId = rank.Id;
                        }
                        await _userManager.UpdateAsync(user);   
                    }
                    //  trừ voucher khi đã sử dụng
                    if (voucherId != new Guid())
                    {
                        var voucher = await _voucherService.GetById(voucherId);
                        voucher.Quantity -= 1;
                        await _voucherService.Update(voucher);
                        var userVoucher = (await _userVoucherService.Gets()).FirstOrDefault(c=>c.VoucherId==voucherId&& order.UserId== c.UserId);
                        userVoucher.Status = -1;
                        await _userVoucherService.Update(userVoucher);
                    }
                    return Json(new { isSuccess = true, message = "Mua hàng thành công", paymentType = "off", url = $"/Admin/BanTaiQuay/CheckOutSuccess?orderId={orderId}" });
                }
                return Json(new { isSuccess = false, message = "Có lỗi sảy ra", paymentType = "off", url = $"/Admin/BanTaiQuay/CheckOutFailed" });

            }
            else if (order.PaymentType == "momo")
            {
                await _orderService.Update(order);
                var url = await UrlCheckOutMoMo(order);
                return Json(new { isSuccess = true, message = "Mua hàng momo", paymentType = paymentType, url = url });
            }
            else
            {
                await _orderService.Update(order);
                return Json(new { isSuccess = true, message = "Mua hàng vnpay", paymentType = paymentType, url = await UrlCheckOutVnPay(order) });
            }
        }
        public async Task<string> UrlCheckOutVnPay(Order order)
        {
            string vnp_Returnurl = $"https://localhost:7116/Admin/BanTaiQuay/PaymentCallBack?orderId={order.Id}"; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "AKU08817"; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = "CBVBDQZOHUERGMDHAQRWSINJIBSCCFTO"; //Secret Key
            string ipAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
            //Get payment input
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.TotalAmout * 100)?.ToString("G29"));
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", ipAddr);
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng: " + order.OrderCode);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return paymentUrl;
        }
        public async Task<string> UrlCheckOutMoMo(Order order)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string orderInfo = "Thanh toán đơn hàng : " + order.OrderCode;
            string redirectUrl = $"https://localhost:7116/Admin/BanTaiQuay/PaymentCallBack?orderId={order.Id}";
            string ipnUrl = $"https://localhost:7116/Admin/BanTaiQuay/PaymentCallBack?orderId={order.Id}";
            string requestType = "captureWallet";

            string amount = order.TotalAmout?.ToString("G29");
            string orderId = order.OrderCode;
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;


            MoMoLibrary crypto = new MoMoLibrary();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Cỏ 4 lá store" },
                { "storeId", "Cỏ 4 lá store" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = MoMoRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            return (jmessage.GetValue("payUrl").ToString());
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack(Guid orderId)
        {
            var isSuccess = false;
            if (Request.Query.Count > 0)
            {
                var order = await _orderService.GetById(orderId);
                if (order.PaymentType == "momo")
                {
                    var resultCode = Request.Query["resultCode"];
                    if (resultCode == "0")
                    {
                        order.OrderStatus = 9; // mua tại quầy
                        order.PaymentDate = DateTime.Now;
                        order.UpdateDate = DateTime.Now;
                        var result = await _orderService.Update(order);
                        if (result)
                        {
                            isSuccess = true;
                        }
                    }
                }
                if (order.PaymentType == "vnpay")
                {
                    string vnp_HashSecret = "CBVBDQZOHUERGMDHAQRWSINJIBSCCFTO"; //Secret Key
                    var vnpayData = Request.Query;
                    VnPayLibrary vnpay = new VnPayLibrary();
                    foreach (var s in vnpayData)
                    {
                        //get all querystring data
                        if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s.Key, vnpayData[s.Key]);
                        }
                    }
                    string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                    string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                    String vnp_SecureHash = Request.Query["vnp_SecureHash"];
                    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                        {

                            order.OrderStatus = 9;
                            order.PaymentDate = DateTime.Now;
                            order.UpdateDate = DateTime.Now;
                            var result = await _orderService.Update(order);
                            if (result)
                            {
                            isSuccess = true;
                            }
                        }
                    }

                }


            }
            if (isSuccess)
            {
                var order = await _orderService.GetById(orderId);
                if (order.UserId != null)//  cộng xu
                {
                    var coinsPlus = (order.TotalAmout + order.VoucherValue + order.CoinsUsed) / 100;
                    var user = await _userManager.FindByIdAsync(order.UserId);
                    user.Coins += coinsPlus;
                    user.Points += (int)(order.TotalAmout + order.VoucherValue + order.CoinsUsed);
                    var rank = (await _ranksService.Gets()).FirstOrDefault(c => c.PointsMin <= user.Points && c.PoinsMax >= user.Points);
                    if (user.RankId != rank.Id)
                    {
                        user.RankId = rank.Id;
                    }
                    await _userManager.UpdateAsync(user);
                }

                if (order.VoucherId != null)
                {
                    var voucher = await _voucherService.GetById((Guid)order.VoucherId);
                    voucher.Quantity -= 1;
                    await _voucherService.Update(voucher);
                    var userVoucher = (await _userVoucherService.Gets()).FirstOrDefault(c => c.VoucherId == order.VoucherId && order.UserId == c.UserId);
                    userVoucher.Status = -1;
                    await _userVoucherService.Update(userVoucher);

                }
                await _hubContext.Clients.All.SendAsync("ReceiveNotification","Đã thanh toán", true);
                return Redirect($"/Admin/BanTaiQuay/CheckOutSuccess?orderId={orderId}");
            }
            return Redirect($"/Admin/BanTaiQuay/CheckOutFailed");

        }
        public async Task<IActionResult> CheckOutSuccess(Guid orderId)
        {
            var order = await _orderService.GetById(orderId);
            return View(order);
        }
        public async Task<IActionResult> CheckOutFailed()
        {
            return View();
        }


    }
}
