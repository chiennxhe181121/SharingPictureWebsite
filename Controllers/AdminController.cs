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
                TempData["Success"] = "User has been banned successfully.";
            }
            catch (Exception)
            {
                TempData["Error"] = "User not found.";
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
                TempData["Success"] = "User has been unbanned successfully.";
            }
            catch (Exception)
            {
                TempData["Error"] = "User not found.";
            }

            return RedirectToAction("Admin", new { page });
        }
    }
}