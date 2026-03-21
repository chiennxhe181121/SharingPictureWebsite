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
        public IActionResult Admin()
        {
            var members = _memberService.GetAllMembers();
            var stats = _memberService.GetDashboardStats();

            ViewBag.TotalUsers = stats.TotalUsers;
            ViewBag.TotalImages = stats.TotalImages;
            ViewBag.ActiveUsers = stats.ActiveUsers;
            ViewBag.BannedUsers = stats.BannedUsers;
            ViewBag.ApprovedImages = stats.ApprovedImages;
            ViewBag.ActivePercent = stats.ActivePercent;
            ViewBag.BannedPercent = stats.BannedPercent;

            return View(members);
        }

        [HttpPost("ban/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Ban(int id)
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

            return RedirectToAction("Admin");
        }

        [HttpPost("unban/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Unban(int id)
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

            return RedirectToAction("Admin");
        }
    }
}