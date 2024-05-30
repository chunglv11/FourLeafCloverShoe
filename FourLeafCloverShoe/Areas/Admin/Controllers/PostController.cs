using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FourLeafCloverShoe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private IPostService _postService;
        private readonly UserManager<User> _userManager;
        public PostController(UserManager<User> userManager, IPostService postService)
        {
            _postService = postService;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName).Trim();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/post", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageList = HttpContext.Session.GetString("TitleImage");
            if (imageList != null)
            {
                HttpContext.Session.Remove("TitleImage");
            }
            var updatedImageList = $"/images/post/{fileName}";
            // Cập nhật danh sách ảnh trong Session
            HttpContext.Session.SetString("TitleImage", updatedImageList);
            return Ok();
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View((await _postService.Gets()).OrderByDescending(c=>c.CreateAt));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Post post)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("chuadangnhap");
            }
            string TitleImage = HttpContext.Session.GetString("TitleImage");
            if (TitleImage == null)
            {
                // Trả về view với thông báo lỗi nếu ModelState không hợp lệ hoặc không có tệp nào được tải lên
                ModelState.AddModelError("", "Vui lòng thêm ảnh.");
                return View(post);
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                post.CreateAt = DateTime.Now;
                post.UserId = user.Id;
                post.TittleImage = TitleImage;
                var createPostResult = await _postService.Add(post);
                if (createPostResult)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(post);
        }
        public async Task<IActionResult> Edit(Guid Id)
        {
            var post = await _postService.GetById(Id);
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(Post post)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("chuadangnhap");
            }
            if (ModelState.IsValid)
            {
                var postDb = await _postService.GetById(post.Id);
                postDb.UpdateAt = DateTime.Now;
                postDb.Contents = post.Contents;
                postDb.Status = post.Status;
                postDb.Tittle = post.Tittle;
                postDb.Description = post.Description;
                var createPostResult = await _postService.Update(postDb);
                if (createPostResult)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(post);
        }
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _postService.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
