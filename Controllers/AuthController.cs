using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;
using System.Security.Claims;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var member = _memberService.Login(username, password);
            if (member == null)
            {
                TempData["Error"] = "Incorrect email/username or password, or the account is locked.";
                return RedirectToAction("Login");
            }

            // Tạo claims
            var avatar = string.IsNullOrEmpty(member.AvatarURL)
                ? "/images/user/default-avatar.jpg"
                : member.AvatarURL;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.MemberID.ToString()),
                new Claim("MemberName", member.MemberName),
                new Claim(ClaimTypes.Role, member.Role.RoleName),
                new Claim("FullName", member.FullName ?? ""),
                new Claim("AvatarUrl", avatar)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(4)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return member.Role.RoleName switch
            {
                "Admin" => RedirectToAction("Admin", "Admin"),
                "Moderator" => RedirectToAction("Moderator", "Moderator"),
                _ => RedirectToAction("Index", "Gallery"),
            };
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
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
