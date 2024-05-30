using FourLeafCloverShoe.Share.Models;
using FourLeafCloverShoe.Share.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text;
using FourLeafCloverShoe.IServices;
using Microsoft.AspNetCore.Identity.UI.Services;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[AdminAreaAuthorization]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ICartService _cartService;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender,
            ICartService cartService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _cartService = cartService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var users = await _userManager.Users.Where(c => /*c.EmailConfirmed == true && */c.UserName != "Guest").ToListAsync();

            var userViewModels = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result.ToList(),
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                DateOfBirth = user.DateOfBirth,
                RankId = user.RankId,
                ProfilePicture = user.ProfilePicture
                // thêm được nữa
            }).ToList();

            return View(userViewModels);
        }
        public async Task<IActionResult> CreateAsync()
        {
            var roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            var roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToListAsync();



            // Truyền danh sách role vào ViewBag hoặc ViewModel để sử dụng trong Razor view
            ViewBag.Roles = roles;
            var usermodel = new User()
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Points = 0,
                Coins = 0,
                RankId = Guid.Parse("2FA0118D-B530-421F-878E-CE4D54BFC6AB")
            };
            var result = await _userManager.CreateAsync(usermodel, user.Password);

            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync(user.Roles.First());
                if (role != null)
                {
                    var roleResult = await _userManager.AddToRolesAsync(usermodel, user.Roles);
                    if (roleResult.Succeeded)
                    {
                        var createCartResult = await _cartService.Add(new Cart() { Id = Guid.NewGuid(), UserId = usermodel.Id });

                        if (createCartResult)
                        {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(usermodel);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            // Construct callback URL for email confirmation
                            var callbackUrl = Url.Action(
                                "ConfirmEmail",                 // Action method for email confirmation
                                "Account",                      // Controller name
                                new
                                {
                                    area = "Identity",          // Area name
                                    userId = usermodel.Id,      // User ID
                                    code = code,                // Confirmation code           // Return URL after confirmation
                                },
                                protocol: Request.Scheme      // Protocol (http or https)
                            );
                            // Send email confirmation email
                            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View(user);
        }


        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
                PhoneNumber = user.PhoneNumber,
                // Thêm các thông tin khác nếu cần
            };
            var roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToListAsync();



            // Truyền danh sách role vào ViewBag hoặc ViewModel để sử dụng trong Razor view
            ViewBag.Roles = roles;

            return View(userViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            var updateu = await _userManager.UpdateAsync(user);
            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update account.");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, model.Roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update account.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LockUnlockAsync(string userId)

        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }


            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, null);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Khoá tài khoản thất bại.");
                }
            }
            else
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Khoá tài khoản thất bại.");
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Xoá tài khoản thất bại.");
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ResendMail(string userId)
        {
            var usermodel = await _userManager.FindByIdAsync(userId);
            if (usermodel == null)
            {
                return NotFound();
            }

            else
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(usermodel);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Construct callback URL for email confirmation
                var callbackUrl = Url.Action(
                    "ConfirmEmail",                 // Action method for email confirmation
                    "Account",                      // Controller name
                    new
                    {
                        area = "Identity",          // Area name
                        userId = usermodel.Id,      // User ID
                        code = code,                // Confirmation code           // Return URL after confirmation
                    },
                    protocol: Request.Scheme      // Protocol (http or https)
                );
                // Send email confirmation email 
                await _emailSender.SendEmailAsync(usermodel.Email, "Xác nhận email",
                    $"Vui lòng xác nhận email của bạn bằng cách <a href='{callbackUrl}'>Nhấn vào đây</a>.");

            return RedirectToAction(nameof(Index));
            }

        }

    }
}
