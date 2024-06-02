using FourLeafCloverShoe.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FourLeafCloverShoe.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
           _postService = postService;
        }
        public async Task<IActionResult> Index()
        {
            var postSell = await _postService.Getssell();
            return View(postSell);
        }
        public async Task<IActionResult> getsNoti()
        {
            var postnoti = await _postService.GetsNoti();
            return View(postnoti);
        }
    }
}
