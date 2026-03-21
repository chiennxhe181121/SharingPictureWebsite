using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;

namespace SharingPictureWebsite.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IMemberService _memberService;

        public AdminController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Admin(int page = 1)
        {
            const int pageSize = 10;
            var model = _memberService.GetAdminDashboard(page, pageSize);

            return View(model);
        }

        [HttpPost("ban/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Ban(int id, int page = 1)
        {
            try
            {
                _memberService.BanMember(id);
                TempData["Success"] = "Da ban nguoi dung thanh cong.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Khong tim thay nguoi dung.";
            }

            return RedirectToAction("Admin", new { page });
        }

        [HttpPost("unban/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Unban(int id, int page = 1)
        {
            try
            {
                _memberService.UnbanMember(id);
                TempData["Success"] = "Da unban nguoi dung thanh cong.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Khong tim thay nguoi dung.";
            }

            return RedirectToAction("Admin", new { page });
        }
    }
}