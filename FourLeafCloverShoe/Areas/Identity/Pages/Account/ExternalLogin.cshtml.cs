// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using FourLeafCloverShoe.IServices;
using System.Net.Mail;
using System.Net;

namespace FourLeafCloverShoe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ICartService _cartService;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            ILogger<ExternalLoginModel> logger,
             ICartService cartService,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _cartService = cartService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        
        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Lỗi từ nhà cung cấp bên ngoài: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Lỗi khi tải thông tin đăng nhập.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Đăng nhập bằng external login provider nếu người dùng đã có tài khoản
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else // Nếu chưa có tài khoản, đăng ký và đăng nhập luôn
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = new User
                    {
                        UserName = new MailAddress(email).User,
                        Email = email,
                        Coins = 0,
                        FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                        RankId = Guid.Parse("2FA0118D-B530-421F-878E-CE4D54BFC6AB"),
                        Points = 0
                    };

                    // Lấy ảnh đại diện (nếu có)
                    var profilePictureUrl = info.Principal.FindFirstValue("picture");
                    if (!string.IsNullOrEmpty(profilePictureUrl))
                    {
                        using var webClient = new WebClient();
                        user.ProfilePicture = webClient.DownloadData(profilePictureUrl);
                    }

                    var createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        createResult = await _userManager.AddLoginAsync(user, info);
                        if (createResult.Succeeded)
                        {
                            // Tạo giỏ hàng
                            await _cartService.Add(new Cart() { Id = Guid.NewGuid(), UserId = user.Id });
                            // Thêm role
                            await _userManager.AddToRoleAsync(user, "User");
                            // Xác nhận email
                            await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
                            // Đăng nhập
                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    // Xử lý nếu tạo người dùng hoặc thêm login thất bại
                }
                // Xử lý nếu không có email
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
        }


        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Lấy thông tin từ external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Lỗi khi tải thông tin đăng nhập từ nhà cung cấp bên ngoài.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                // Tìm người dùng bằng email
                var existingUser = await _userManager.FindByEmailAsync(Input.Email);

                if (existingUser == null) // Nếu người dùng chưa tồn tại
                {
                    // Tạo người dùng mới
                    var user = new User
                    {
                        UserName = new MailAddress(Input.Email).User,
                        Email = Input.Email,
                        Coins = 0,
                        FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                        RankId = Guid.Parse("2FA0118D-B530-421F-878E-CE4D54BFC6AB"),
                        Points = 0
                    };

                    // Lấy ảnh đại diện (nếu có)
                    var profilePictureUrl = info.Principal.FindFirstValue("picture"); // Google sử dụng ClaimTypes.Uri
                    if (!string.IsNullOrEmpty(profilePictureUrl))
                    {
                        using var webClient = new WebClient();
                        user.ProfilePicture = webClient.DownloadData(profilePictureUrl);
                    }

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return Page(); // Trả về trang với lỗi nếu tạo người dùng thất bại
                    }

                    // Thêm login (external provider) cho người dùng
                    result = await _userManager.AddLoginAsync(user, info);
                    if (!result.Succeeded)
                    {
                        ErrorMessage = $"Có lỗi xảy ra, vui lòng thử lại sau!";
                    }

                    existingUser = user; // Gán existingUser bằng user mới tạo
                }
                else // Nếu người dùng đã tồn tại
                {
                    // Thêm login mới cho người dùng đã tồn tại
                    var result = await _userManager.AddLoginAsync(existingUser, info);
                    if (!result.Succeeded)
                    {
                        ErrorMessage = $"Có lỗi xảy ra, vui lòng thử lại sau!";
                    }
                }
                _logger.LogInformation("Người dùng đã tạo tài khoản bằng {Name} provider.", info.LoginProvider);

                var userId = existingUser.Id; // Sử dụng ID của existingUser

                // Tạo giỏ hàng
                var createCartResult = await _cartService.Add(new Cart() { Id = Guid.NewGuid(), UserId = userId });

                // Thêm vai trò
                await _userManager.AddToRoleAsync(existingUser, "User");

                // Tự động xác nhận email
                await _userManager.ConfirmEmailAsync(existingUser, await _userManager.GenerateEmailConfirmationTokenAsync(existingUser));

                // Đăng nhập người dùng
                await _signInManager.SignInAsync(existingUser, isPersistent: false, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
