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
    int pageSize = 6)
        {
            var model = _service.GetPublicGallery(search, categoryId, sortBy, page, pageSize);
            model.PageSize = pageSize; 
            return View(model);
        }

        [HttpGet("image/{id}")]
        public IActionResult ImageDetail(int id)
        {
            int currentMemberId = 2; // tạm gán member id 2

            var model = _service.GetPictureDetail(id, currentMemberId);
            if (model == null)
                return NotFound();

            return View(model); // bind cho Image Detail view
        }
    }
}