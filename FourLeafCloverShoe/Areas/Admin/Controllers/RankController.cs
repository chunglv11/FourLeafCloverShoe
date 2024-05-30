using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace FourLeafCloverShoe.Controllers
{
    [Area("Admin")]

    public class RankController : Controller
    {

        private IRanksService _iranksService;

        public RankController(IRanksService ranksService)
        {
            _iranksService = ranksService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _iranksService.Gets()).OrderBy(c => c.PoinsMax));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Ranks obj)
        {
            var ranks = await _iranksService.Gets();
            var existingRank = ranks.FirstOrDefault(c => c.Name.Trim().ToLower() == obj.Name.Trim().ToLower());

            if (existingRank != null)
            {
                TempData["ErrorMessage"] = "Đã có rank này";
                return View();
            }

            if (obj.PointsMin >= obj.PoinsMax)
            {
                TempData["ErrorMessage"] = "Điểm Min phải nhỏ hơn điểm Max";
                return View(obj);
            }

            // Kiểm tra xem obj.PointsMin và obj.PoinsMax có nằm trong khoảng của các rank có sẵn không
            foreach (var rank in ranks)
            {
                if (obj.PointsMin >= rank.PointsMin && obj.PointsMin <= rank.PoinsMax)
                {
                    TempData["ErrorMessage"] = "Điểm Min phải nằm ngoài khoảng Min Max có sẵn";
                    return View(obj);
                }

                if (obj.PoinsMax >= rank.PointsMin && obj.PoinsMax <= rank.PoinsMax)
                {
                    TempData["ErrorMessage"] = "Điểm Max phải nằm ngoài khoảng Min Max có sẵn";
                    return View(obj);
                }
            }

            // Thêm rank mới vào danh sách
            var result = await _iranksService.Add(obj);
            if (result)
            {
                TempData["SuccessMessage"] = "Thêm thành công";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid Id)
        {

            return View(await _iranksService.GetById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, Ranks obj)
        {
            if (obj == null)
            {
                TempData["ErrorMessage"] = "Không được để trống";
                return View(obj);
            }

            var ranks = await _iranksService.Gets();
            var rank = await _iranksService.GetById(Id);

            if (ranks.Any(c => c.Name.Trim().ToLower() == obj.Name.Trim().ToLower()))
            {
                TempData["ErrorMessage"] = "Đã Có Rank Này";
                return View(obj);
            }

            if (obj.PointsMin >= rank.PointsMin && obj.PointsMin <= rank.PoinsMax)
            {
                TempData["ErrorMessage"] = "Điểm Min phải nằm ngoài khoảng Min Max có sẵn";
                return View(obj);
            }

            if (obj.PoinsMax >= rank.PointsMin && obj.PoinsMax <= rank.PoinsMax)
            {
                TempData["ErrorMessage"] = "Điểm Max phải nằm ngoài khoảng Min Max có sẵn";
                return View(obj);
            }

            if (obj.PointsMin >= obj.PoinsMax)
            {
                TempData["ErrorMessage"] = "Điểm Min phải nhỏ hơn điểm Max";
                return View(obj);
            }

            rank.Name = obj.Name;
            var result = await _iranksService.Update(rank);
            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Cập nhật không thành công";
            return View(obj);
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _iranksService.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}

