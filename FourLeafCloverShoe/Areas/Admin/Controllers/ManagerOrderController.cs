using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using X.PagedList;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerOrderController : Controller
    {
        private readonly IOrderService _iorderService;
        private readonly IOrderItemService _iorderItemService;
        private readonly IProductDetailService _productDetailService;
        private readonly IProductService _productService;
        private readonly ISizeService _sizeService;
        private readonly UserManager<User> _userManager;

        public ManagerOrderController(IOrderService iorderService, IOrderItemService iorderItemService, IProductDetailService productDetailService, IProductService productService, ISizeService sizeService, UserManager<User> userManager)
        {
            _iorderService = iorderService;
            _iorderItemService = iorderItemService;
            _productDetailService = productDetailService;
            _productService = productService;
            _sizeService = sizeService;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync(int? page, int?[] status, string searchText, DateTime? startDate, DateTime? endDate)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            // Lưu trữ giá trị bộ lọc vào ViewBag để sử dụng trong View
            ViewBag.SelectedStatuses = status;
            ViewBag.searchText = searchText;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;

            var lst = await _iorderService.Gets();
            var lstOrder = lst.Where(c => c.Id != null);
            // Lọc theo searchText 
            if (!string.IsNullOrEmpty(searchText))
            {
                lstOrder = lstOrder.Where(c => c.OrderCode.ToLower().Contains(searchText.ToLower()));
            }

            // Lọc theo status 
            if (status != null && status.Length > 0)
            {
                lstOrder = lstOrder.Where(c => status.Contains(c.OrderStatus));
            }

            // Lọc theo ngày 
            if (startDate != null || endDate != null)
            {
                // Nếu chỉ có startDate
                if (startDate != null && endDate == null)
                {
                    lstOrder = lstOrder.Where(c => c.CreateDate >= startDate);
                }
                // Nếu chỉ có endDate
                else if (startDate == null && endDate != null)
                {
                    lstOrder = lstOrder.Where(c => c.CreateDate <= endDate);
                }
                // Nếu có cả startDate và endDate
                else if (startDate != null && endDate != null)
                {
                    lstOrder = lstOrder.Where(c => c.CreateDate >= startDate && c.CreateDate <= endDate);
                }
            }
            return View(lstOrder.ToPagedList(pageNumber, pageSize));
        }


        public async Task<IActionResult> OrderDetail(Guid orderId, string? keyWord, int pageNumber = 1, int pageSize = 5)
        {
            var lstOrderIterm = await _iorderItemService.GetByIdOrder2(orderId);
            var lstProductDetail = (await _productDetailService.GetProductDetails()).Where(c => c.StatusPro == 1 && c.Quantity > 0).ToList();        
            if (!string.IsNullOrEmpty(keyWord))
            {
                lstProductDetail = lstProductDetail.Where(p => !string.IsNullOrEmpty(p.ProductName) && p.ProductName.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToList();
            }
            ViewBag.lstProduct = lstProductDetail.ToPagedList(pageNumber, pageSize);
            ViewBag.Keyword = keyWord;
            return View(lstOrderIterm);
        }
        
        public async Task<IActionResult> GetProductDetail()
        {
            int pageNumber = 1;
            int pageSize = 6;
            var lstProductDetail = (await _productDetailService.GetProductDetails()).Where(c => c.StatusPro == 1 && c.Quantity > 0).ToList();
            ViewBag.lstProduct = lstProductDetail.ToPagedList(pageNumber, pageSize);
            return Json(new { success = true, data = lstProductDetail });
        }

        public async Task<IActionResult> DoiTrangThai(Guid idhd, int trangthai)// Dùng cho trạng thái truyền  vào: 10, 3
        {

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;


                if (identity != null)
                {
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value); // userId


                    var idnv = userID.ToString();

                    if (trangthai == 3) // chờ lấy hàng
                    {
                        var hoadonCT = await _iorderItemService.GetByIdOrder(idhd);
                        foreach (var item in hoadonCT)
                        {
                            var responseUpdateQuantityProductDetail = await _productDetailService.UpdateQuantityById(item.ProductDetailID, item.Quantity);
                            if (responseUpdateQuantityProductDetail == false)
                            {// xác nhận đơn xong thì mới trừ số lượng sp
                                return BadRequest();
                            }
                        }
                        var updateSLSPfromDb = await _productService.UpdateSLTheoSPCT();
                        if (updateSLSPfromDb == false)
                        { // update lại slsp
                            return BadRequest();

                        }


                        var response = await _iorderService.UpdateOrderStatus(idhd, trangthai, idnv);
                        if (response == true)
                        {


                            return Json(new { success = true, message = "Cập nhật trạng thái thành công" });

                        }
                    } 
                    else if (trangthai == 8)
                    {
                        var response =  _iorderService.ThanhCong(idhd, idnv);
                        if (response == true)
                        {


                            return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                            

                        }
                    }
                    else 
                    {
                        var response = await _iorderService.UpdateOrderStatus(idhd, trangthai, idnv);
                        if (response == true)
                        {


                            return Json(new { success = true, message = "Cập nhật trạng thái thành công" });

                        }

                    }


                }
                return Json(new { success = true, message = "Cập nhật trạng thái thất bại " });
             

            }
            catch (Exception)
            {
                return RedirectToAction("OrderDetail", "ManagerOrder");
            }
        }
        [HttpGet("/Order/HuyHD")]
        public async Task<IActionResult> HuyHD(Guid idhd, string ghichu)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value); // userId


                    var idnv = userID.ToString();
                    if (ghichu != null)
                    {
                        var response = await _iorderService.HuyHD(idhd, idnv);
                        if (response == true)
                        {
                        var responseghichu =  _iorderService.UpdateGhiChuHD(idhd, idnv, ghichu);

                            if (responseghichu = true)
                            {
                                ViewBag.SuccessMessage = "Cập nhật trạng thái thành công";
                                return RedirectToAction("OrderDetail");


                            }
                        }
                    }
                    ViewBag.ErrorMessage = "Ghi chú không được trống";

                }
                return RedirectToAction("OrderDetail");



            }
            catch (Exception ex)
            {
                return RedirectToAction("OrderDetail", "ManagerOrder");

            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProductDetailToOrder(Guid orderId, Guid productDetailId, int Quantity)
        {
            var order = await _iorderService.GetById(orderId);
            var productDetail = await _productDetailService.GetById(productDetailId);
            var OrderItem = (await _iorderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
            if (order != null && productDetail != null)
            {

                if (productDetail.Status != 1 || productDetail.Products.Status != true)
                {

                    return Json(new { success = false, message = "Sản phẩm ngừng kinh doanh!" });
                }
                if (productDetail.Quantity + OrderItem.Quantity < Quantity)
                {
                    return Json(new { message = "Sản phẩm vượt quá giới hạn trong kho", success = false });
                }
                if (productDetail.Quantity < Quantity)
                {
                    return Json(new { message = "Sản phẩm vượt quá giới hạn trong kho", success = false });
                }
                if (Quantity <= 0)
                {
                    return Json(new { message = "Sản phẩm tối thiểu là 1", success = false });
                }
                else
                {
                    var lstOrderItems = (await _iorderItemService.Gets()).Where(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
                    
                    if (!lstOrderItems.Any()) // nếu sp chưa có trong orderiterm thì thêm mới
                    {
                        var orderItem = new OrderItem()
                        {
                            Id = Guid.NewGuid(),
                            ProductDetailId = productDetail.Id,
                            OrderId = order.Id,
                            Quantity = 1,
                            Price = productDetail.PriceSale
                        };
                        var resultCreateOrderItem = await _iorderItemService.Add(orderItem);
                        productDetail.Quantity -= 1;
                        await _productDetailService.Update(productDetail);
                        await UpdateOrder(order.Id);
                        return Json(new { success = true, message = "Thêm sản phẩm thành công", Id = orderItem.Id, soluong = orderItem.Quantity, total = order.TotalAmout });
                    }
                    else // cộng dồn số lượng
                    {
                        var orderItem = (await _iorderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
                        orderItem.Quantity += Quantity;
                        var resultCreateOrderItem = await _iorderItemService.Update(orderItem);
                        productDetail.Quantity -= Quantity;
                        await _productDetailService.Update(productDetail);
                        await UpdateOrder(order.Id);
                        return Json(new { success = true, message = "Thêm sản phẩm thành công", Id = orderItem.Id, soluong = orderItem.Quantity, total = order.TotalAmout });
                    }
                }
            }
            return Json(new { success = false, message = "Lỗi không xác định" });
        }
        public async Task<IActionResult> UpdateOrder(Guid orderId)
        {
            try
            {
                var hd = await _iorderService.GetById(orderId);
                if (hd != null)
                {
                    var orderItems = (await _iorderItemService.Gets()).Where(c => c.OrderId == orderId);
                    if (orderItems.Count() > 0)
                    {
                        var user = await _userManager.GetUserAsync(HttpContext.User);
                        // Tính lại tổng tiền hóa đơn
                        decimal? totalAmount = orderItems.Sum(item => item.Quantity * item.Price) + hd.ShippingFee;
                        hd.UpdateDate = DateTime.Now;
                        hd.StaffId = user.Id;
                        hd.TotalAmout = totalAmount;
                        var update = await _iorderService.Update(hd);
                        if (update != null)
                        {
                            return Json(new { success = true, message = " Cập nhật hoá đơn thành công" });
                        }

                    }
                    else
                    {
                        return Json(new { success = true, message = "Hóa đơn không có sản phẩm nào không thể cập nhập" });

                    }
                }
                else
                {
                    return Json(new { success = true, message = " Cập nhật hoá đơn thất bại" });
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> Remove(Guid productDetailId, Guid orderId)
        {
            var order = await _iorderService.GetById(orderId);
            var orderItem = (await _iorderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
            if (orderItem != null)
            {
                var result = await _iorderItemService.Delete(orderItem.Id);
                if (result)
                {
                    var productDetail = await _productDetailService.GetById(productDetailId);
                    productDetail.Quantity += orderItem.Quantity;
                    await _productDetailService.Update(productDetail);
                    await UpdateOrder(order.Id);
                    return Json(new { message = "Xoá thành công", success = true });
                }
            }
            return Json(new { message = "Lỗi không xác định", success = false });
        }
        public async Task<IActionResult> TangSL(Guid orderId, Guid productDetailId)
        {
            
            try
            {
                var productDetail = await _productDetailService.GetById(productDetailId);
                var orderItem = (await _iorderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
                if (orderItem.Quantity > productDetail.Quantity)
                {
                    return Json(new { message = "Số lượng vượt quá sản phẩm trong kho", success = false });
                }
                else
                {
                    orderItem.Quantity += 1;
                    await _iorderItemService.Update(orderItem);
                    productDetail.Quantity -= 1;
                    await _productDetailService.Update(productDetail);
                    await UpdateOrder(orderId);
                    return Json(new { message = "Thêm thành công", success = true });
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> GiamSL(Guid orderId, Guid productDetailId)
        {

            try
            {
                var productDetail = await _productDetailService.GetById(productDetailId);
                var orderItem = (await _iorderItemService.Gets()).FirstOrDefault(c => c.ProductDetailId == productDetailId && c.OrderId == orderId);
                if (orderItem.Quantity <= 1)
                {
                    return Json(new { message = "Số lượng nhỏ nhất là 1", success = false });
                }
                else
                {
                    orderItem.Quantity -= 1;
                    await _iorderItemService.Update(orderItem);
                    productDetail.Quantity += 1;
                    await _productDetailService.Update(productDetail);
                    await UpdateOrder(orderId);
                    return Json(new { message = "Giảm thành công", success = true });
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
