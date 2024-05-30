using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class OrderDetailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderService _orderService;
        private readonly IRateService _rateService;
        public Order Order { get; set; }
        public Rate Rate { get; set; }
        public OrderDetailModel(UserManager<User> userManager, IOrderService orderService, IRateService rateService)
        {

            _userManager = userManager;
            _orderService = orderService;
            _rateService = rateService;
        }
        public async Task<IActionResult> OnGetAsync(Guid orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            Order = (await _orderService.Gets()).FirstOrDefault(c => c.UserId == user.Id&&c.Id==orderId);
            return Page();
        }
        public async Task<IActionResult> OnPostHuyDonAsync(Guid orderId, string selectedReason)
        {
            var order =await _orderService.GetById(orderId);
            order.OrderStatus = 13; // đã huỷ đơn
            order.Description = selectedReason;
            order.UpdateDate = DateTime.Now;
            await _orderService.Update(order);
            return Redirect($"/Identity/Account/Manage/orderdetail?orderId={order.Id}");
        }
        public async Task<IActionResult> OnPostYeuCauHuyDonAsync(Guid orderId, string selectedReason)
        {
            var order =await _orderService.GetById(orderId);
            order.OrderStatus = 5;  // yêu cầu huỷ đơn 
            order.Description = selectedReason;
            order.UpdateDate = DateTime.Now;
            await _orderService.Update(order);
            return Redirect($"/Identity/Account/Manage/orderdetail?orderId={order.Id}");
        }
        public async Task<IActionResult> OnPostYeuCauHoanDonAsync(Guid orderId, string selectedReason)
        {
            var order =await _orderService.GetById(orderId);
            order.OrderStatus = 10;  // yêu cầu hoàn đơn
            order.Description = selectedReason;
            order.UpdateDate = DateTime.Now;
            await _orderService.Update(order);
            return Redirect($"/Identity/Account/Manage/orderdetail?orderId={order.Id}");
        }
        


    }
}
