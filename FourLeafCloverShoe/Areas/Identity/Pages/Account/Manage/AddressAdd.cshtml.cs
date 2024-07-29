using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class AddressAddModel : PageModel
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<User> _userManager;

        public AddressAddModel(UserManager<User> userManager, IAddressService addressService)
        {
            _userManager = userManager;
            _addressService = addressService;
        }

        [BindProperty]
        public Address Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Không thể tải người dùng với ID '{_userManager.GetUserId(User)}'.");
            }

            Input.UserId = user.Id;
            bool isDefault = Request.Form["IsDefault"] == "true";
            Input.IsDefault = isDefault;

            var result = await _addressService.Add(Input);

            if (result)
            {
                if (Input.IsDefault)
                {
                    var setDefaultResult = await _addressService.SetDefault(Input.Id);
                    if (!setDefaultResult)
                    {
                        ModelState.AddModelError(string.Empty, "Không thể đặt địa chỉ mặc định.");
                        return Page();
                    }
                }

                return RedirectToPage("./Address");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể thêm địa chỉ. Vui lòng thử lại sau.");
                return Page();
            }
        }
    }
}
