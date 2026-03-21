using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;

namespace SharingPictureWebsite.Controllers
{
    [Route("moderator")]
    public class ModeratorController : Controller
    {
        private readonly IPictureService _pictureService;

        public ModeratorController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Moderator(string? status = "all", int page = 1)
        {
            var pagedPictures = _pictureService.GetModeratorPicturesPaged(status, page);
            var stats = _pictureService.GetModeratorStatusStats();

            ViewBag.CurrentFilter = status ?? "all";
            ViewBag.PendingCount = stats.PendingCount;
            ViewBag.ApprovedCount = stats.ApprovedCount;
            ViewBag.RejectedCount = stats.RejectedCount;

            return View(pagedPictures);
        }

        [HttpPost("approve/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            try
            {
                _pictureService.ApprovePicture(id);
                TempData["Success"] = "Duyet anh thanh cong!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Khong tim thay anh de duyet.";
            }

            return RedirectToAction("Moderator");
        }

        [HttpPost("reject/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            try
            {
                _pictureService.RejectPicture(id);
                TempData["Success"] = "Tu choi anh thanh cong!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Khong tim thay anh de tu choi.";
            }

            return RedirectToAction("Moderator");
        }
    }
}