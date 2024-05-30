using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class AddressAddModel : PageModel
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<User> _userManager;

        public AddressAddModel(UserManager<User> userManager, IAddressService addressService)
        {
            _addressService = addressService;
            _userManager = userManager;
        }
        [BindProperty]
        public Address Input { get; set; }
        public async Task<IActionResult> OnGet()
        {

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                Input.UserId = user.Id;

                var result =  await _addressService.Add(Input);
                if (result)
                {
                    if (Input.IsDefault)
                    {
                       var a= await _addressService.SetDefault(Input.Id);

                    }
                    return RedirectToPage("./Address");
                }
            }
            return Page();
            
        }
    }
}
