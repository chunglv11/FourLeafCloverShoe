using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account.Manage
{
    public class AddressModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAddressService _addressService;

        public AddressModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAddressService addressService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _addressService = addressService;
        }
    
        [BindProperty]
        public List<Address> ListAddress { get; set; }
        private async Task LoadAsync(User user)
        {
            var getListAddress = await _addressService.GetByUserId(user.Id);

            ListAddress = getListAddress;
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            await LoadAsync(user);
            return Page();
        }
        public async Task<IActionResult> OnPostSetDefaultAsync(Guid Id)
        {
            var result = await _addressService.SetDefault(Id);
            if (result)
            {
            return RedirectToPage("./Address");
            }
            return NotFound();
        }
        public async Task<IActionResult> OnPostDeleteAsync(Guid idDelete)
        {
            var result = await _addressService.Delete(idDelete);
            if (result)
            {
            return RedirectToPage("./Address");
            }
            return NotFound();
        }

    }
}
