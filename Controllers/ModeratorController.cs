using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Models;
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
        public IActionResult Moderator()
        {
            var pictures = _pictureService
                .GetAllPictures()
                .OrderByDescending(p => p.UploadDate)
                .ToList();

            return View(pictures);
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                TempData["Error"] = "Khong tim thay anh de tu choi.";
            }

            return RedirectToAction("Moderator");
        }
    }
}