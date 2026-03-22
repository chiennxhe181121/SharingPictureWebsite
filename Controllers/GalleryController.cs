using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingPictureWebsite.Services.Interfaces;
using SharingPictureWebsite.ViewModels;
using System.Security.Claims;

namespace SharingPictureWebsite.Controllers
{
    [Route("gallery")]
    public class GalleryController : Controller
    {
        private readonly IPictureService _service;

        // --- Helper lấy MemberID từ claim ---
        private int GetCurrentMemberId()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var memberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(memberIdClaim) && int.TryParse(memberIdClaim, out int memberId))
                {
                    return memberId;
                }
            }
            return 0; // 0 hoặc throw nếu muốn bắt lỗi khi chưa đăng nhập
        }

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
            int currentMemberId = GetCurrentMemberId();
            var model = _service.GetPictureDetail(id, currentMemberId);
            if (model == null)
                return NotFound();

            return View(model); // bind cho Image Detail view
        }

        [HttpPost("image/{id}/like")]
        public IActionResult ToggleLike(int id)
        {
            int currentMemberId = GetCurrentMemberId();
            bool isLiked = _service.ToggleLike(id, currentMemberId);
            int likeCount = _service.GetLikeCount(id);
            return Json(new { success = true, isLiked, likeCount });
        }

        [HttpGet("image/{id}/comments")]
        public IActionResult GetComments(int id, int page = 1)
        {
            var (comments, total) = _service.GetCommentsPaged(id, page, 5);

            return Json(new { comments, total });
        }

        [HttpPost("image/{id}/comment")]
        public IActionResult AddComment(int id, [FromBody] CommentRequest request)
        {
            int currentMemberId = GetCurrentMemberId();
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Empty comment");

            var comment = _service.AddComment(id, currentMemberId, request.Content);

            if (comment == null)
                return BadRequest("Cannot add comments");

            return Json(new
            {
                success = true,
                comment = new
                {
                    userName = comment.Member.FullName,
                    content = comment.Content,
                    createdAt = comment.CreatedAt.ToString("g"),
                    avatarUrl = string.IsNullOrEmpty(comment.Member.AvatarURL)
    ? Url.Content("~/images/user/default-avatar.jpg")
    : comment.Member.AvatarURL.StartsWith("http")
        ? comment.Member.AvatarURL
        : Url.Content("~/" + comment.Member.AvatarURL)
                }
            });
        }
    }
}