using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;

namespace SharingPictureWebsite.Controllers
{
    [Authorize(Roles = "Moderator")]
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
            var dashboard = _pictureService.GetModeratorDashboard(status, page);
            return View(dashboard);
        }

        [HttpPost("approve/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            try
            {
                _pictureService.ApprovePicture(id);
                TempData["Success"] = "Image approved successfully!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Image not found for approval.";
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
                TempData["Success"] = "Image rejected successfully!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Image not found for rejection.";
            }

            return RedirectToAction("Moderator");
        }
    }
}