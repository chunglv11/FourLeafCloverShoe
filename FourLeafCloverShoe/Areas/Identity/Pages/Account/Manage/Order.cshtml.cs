using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class OrderModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderService _orderService;

        public OrderModel(UserManager<User> userManager, IOrderService orderService)
        {
           
            _userManager = userManager;
            _orderService = orderService;
        }
        public IEnumerable<Order> ListOrder { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            ListOrder = (await _orderService.Gets()).Where(c=>c.UserId==user.Id&& c.OrderStatus!=-1).OrderByDescending(c => c.UpdateDate);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int type)
        {
            ViewData["type"] = type;
            var user = await _userManager.GetUserAsync(User);
            ListOrder = (await _orderService.Gets()).Where(c => c.UserId == user.Id).OrderByDescending(c=>c.UpdateDate);
            if (type == 1)
            {
                ListOrder = ListOrder.Where(c => c.OrderStatus == 0 || c.OrderStatus == 1 || c.OrderStatus == 2);
            }
            else if (type == 2)
            {
                ListOrder = ListOrder.Where(c => c.OrderStatus == 3);
            }
            else if (type == 3)
            {
                ListOrder = ListOrder.Where(c => c.OrderStatus == 4 || c.OrderStatus == 5 || c.OrderStatus == 6 || c.OrderStatus == 7);
            }
            else if (type == 4)
            {
                ListOrder = ListOrder.Where(c => c.OrderStatus == 8 || c.OrderStatus == 9 );
            }
            else if (type == 5)
            {
                ListOrder = ListOrder.Where(c => c.OrderStatus == 13 || c.OrderStatus == 14);
            }
            else if (type == 6)
            {
                ListOrder = ListOrder.Where(c => c.OrderStatus == 10 || c.OrderStatus == 11 || c.OrderStatus == 12);
            }

            return Page();
        }
    }
}
