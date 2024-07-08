using FourLeafCloverShoe.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
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

        public ManagerOrderController(IOrderService iorderService, IOrderItemService iorderItemService, IProductDetailService productDetailService, IProductService productService)
        {
            _iorderService = iorderService;
            _iorderItemService = iorderItemService;
            _productDetailService = productDetailService;
            _productService = productService;
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


        public async Task<IActionResult> OrderDetail(Guid orderId)
        {
            var lstOrderIterm = await _iorderItemService.GetByIdOrder2(orderId);
            return View(lstOrderIterm);
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
    }
}
