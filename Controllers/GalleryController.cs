using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;

namespace SharingPictureWebsite.Controllers
{
    [Route("gallery")]
    public class GalleryController : Controller
    {
        private readonly IPictureService _service;

        public GalleryController(IPictureService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Index(
    string? search,
    int? categoryId,
    string? sortBy,
    int page = 1,
    int pageSize = 9)
        {
            var model = _service.GetPublicGallery(search, categoryId, sortBy, page, pageSize);
            model.PageSize = pageSize; // 🔥 thêm dòng này
            return View(model);
        }
    }
}