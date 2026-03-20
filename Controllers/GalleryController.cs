using Microsoft.AspNetCore.Mvc;

namespace SharingPictureWebsite.Controllers
{
    [Route("gallery")]
    public class GalleryController : Controller
    {
        public IActionResult Gallery()
        {
            return View();
        }
    }
}
