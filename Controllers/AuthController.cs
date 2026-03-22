using Microsoft.AspNetCore.Mvc;

namespace SharingPictureWebsite.Controllers
{
    [Route("")]
    public class AuthController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
