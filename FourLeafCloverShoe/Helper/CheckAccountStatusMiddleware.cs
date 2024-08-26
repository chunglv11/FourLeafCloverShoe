using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;

namespace FourLeafCloverShoe.Helper
{

    public class CheckAccountStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckAccountStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            if (context.User.Identity.IsAuthenticated) // check đăng nhập hợp lệ
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user == null || user.LockoutEnd != null) // nếu không hợp lệ thì đăng xuất
                {
                    await signInManager.SignOutAsync();
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }

}
