using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class AddressUpdateModel : PageModel
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<User> _userManager;
        public AddressUpdateModel(UserManager<User> userManager, IAddressService addressService)
        {
            _addressService = addressService;
            _userManager = userManager;
        }
        [BindProperty]
        public Address Input { get; set; }
        public async Task<IActionResult> OnGet(Guid id)
        {
            Input = await _addressService.GetById(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var result = await _addressService.Update(Input);
                if (result)
                {
                    if (Input.IsDefault)
                    {
                        var a = await _addressService.SetDefault(Input.Id);

                    }
                    return RedirectToPage("./Address");
                }
            }
            return Page();

        }
    }
}
