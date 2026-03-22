using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Controllers
{
    [Route("")]
    public class AuthController : Controller
    {
        private readonly IMemberService _memberService;

        public AuthController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View(new RegisterRequestViewModel());
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterRequestViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = _memberService.Register(request);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Registration failed.");
                return View(request);
            }

            TempData["Success"] = "Registration successful. Please sign in.";
            return RedirectToAction(nameof(Login));
        }
    }
}
