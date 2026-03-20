using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services;
using SharingPictureWebsite.Services.Interfaces;

namespace SharingPictureWebsite.Controllers
{
    [Route("picture")]
    public class PictureController : Controller
    {
        private readonly IPictureService _service;

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
                    TempData["Error"] = "Vui lòng chọn file!";
                    return RedirectToAction("Upload");
                }

                int memberId = 1;

                await _service.UploadImageAsync(file, title, description, categoryId, memberId, albumId);

                TempData["Success"] = "Upload thành công!";
                return RedirectToAction("Upload");
            }
            catch
            {
                TempData["Error"] = "Upload thất bại!";
                return RedirectToAction("Upload");
            }
        }
    }
}