using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class OrderItemModel : PageModel
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemModel(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        //public OrderIterm OrderIterms { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid idCTHD)
        {
            var lstOr = (await _orderItemService.Gets()).FirstOrDefault(c => c.Id == idCTHD);
            ViewData["OrderIterm"] = lstOr;
            return Page();
        }
    }
}
