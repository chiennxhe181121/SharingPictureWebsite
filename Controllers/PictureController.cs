using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services;
using SharingPictureWebsite.Services.Interfaces;
using System.Security.Claims;

namespace SharingPictureWebsite.Controllers
{
    [Route("picture")]
    public class PictureController : Controller
    {
        private readonly IPictureService _service;

        // --- Helper lấy MemberID từ claim ---
        private int GetCurrentMemberId()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var memberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(memberIdClaim) && int.TryParse(memberIdClaim, out int memberId))
                {
                    return memberId;
                }
            }
            return 0; // 0 hoặc throw nếu muốn bắt lỗi khi chưa đăng nhập
        }

        public PictureController(IPictureService service)
        {
            _service = service;
        }

        // ================= VIEW UPLOAD =================
        // GET: /picture/upload
        [HttpGet("upload")]
        public IActionResult Upload()
        {
            var model = _service.GetUploadData();
            return View(model);
        }

        // ================= HANDLE UPLOAD =================
        // POST: /picture/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
     IFormFile file,
     string title,
     string? description,
     int categoryId,
     int? albumId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    TempData["Error"] = "Please select an image!";
                    return RedirectToAction("Upload");
                }

                if (string.IsNullOrWhiteSpace(title))
                {
                    TempData["Error"] = "Title cannot be empty!";
                    return RedirectToAction("Upload");
                }

                int memberId = GetCurrentMemberId();

                await _service.UploadImageAsync(file, title, description, categoryId, memberId, albumId);

                TempData["Success"] = "Upload successful! Waiting for moderation.";
                return RedirectToAction("Upload");
            }
            catch
            {
                TempData["Error"] = "An error occurred during upload!";
                return RedirectToAction("Upload");
            }
        }
    }
}