using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;

namespace SharingPictureWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPost("update-role/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRole(int id, int roleId, int page = 1)
        {
            try
            {
                // Lấy ID của admin hiện tại từ claims
                var currentAdminId = int.TryParse(
                    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "-1",
                    out var id_result) ? id_result : -1;

                // Admin không được cấp role cho chính mình
                if (currentAdminId == id)
                {
                    TempData["Error"] = "You cannot change your own role.";
                    return RedirectToAction("Admin", new { page });
                }

                _memberService.UpdateMemberRole(id, roleId);
                TempData["Success"] = "User role has been updated successfully.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Failed to update user role.";
            }

            return RedirectToAction("Admin", new { page });
        }
    }
}