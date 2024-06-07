using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
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
        public async Task<IActionResult> show(Guid Id)
        {
            var ressult = await _postService.GetById(Id);
            return View(ressult);
        }
    }
}
