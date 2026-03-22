using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly AppDbContext _context;

        public PictureRepository(AppDbContext context)
        {
            _context = context;
        }
        public (List<PictureDTO>, int totalItems) GetPublicPictures(
    string? search,
    int? categoryId,
    string? sortBy,
    int page,
    int pageSize)
        {
            var query = _context.Pictures
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Where(p => p.Status == Status.Public)
                .AsQueryable();

            // 🔍 SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Title.Contains(search) ||
                    p.Category!.CategoryName.Contains(search));
            }

            // 🎯 FILTER
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId);
            }

            // 🔽 SORT
            query = sortBy switch
            {
                "oldest" => query.OrderBy(p => p.UploadDate),
                "title" => query.OrderBy(p => p.Title),
                _ => query.OrderByDescending(p => p.UploadDate)
            };

            var totalItems = query.Count();

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PictureDTO
                {
                    PictureID = p.PictureID,
                    Title = p.Title,
                    ImageURL = p.ImageURL,
                    CategoryName = p.Category!.CategoryName,
                    AuthorName = p.Author.FullName,
                    UploadDate = p.UploadDate
                })
                .ToList();

            return (data, totalItems);
        }

        public IEnumerable<Picture> GetAll()
        {
            return _context.Pictures
                .Include(p => p.Author)
                .Include(p => p.Category)
                .ToList();
        }

        private string NormalizeAvatar(string? avatarUrl)
        {
            if (string.IsNullOrEmpty(avatarUrl))
                return "/images/user/default-avatar.jpg";

            if (avatarUrl.StartsWith("http"))
                return avatarUrl;

            if (!avatarUrl.StartsWith("/"))
                return "/" + avatarUrl;

            return avatarUrl;
        }
        public ImageDetailViewModel? GetPictureDetail(int pictureId, int currentMemberId)
        {
            var data = _context.Pictures
                .Where(p => p.PictureID == pictureId)
                .Select(p => new
                {
                    p.PictureID,
                    p.Title,
                    p.ImageURL,
                    p.Description,
                    CategoryName = p.Category != null ? p.Category.CategoryName : "No Category",
                    AuthorName = p.Author.FullName,
                    CreatedAt = p.UploadDate,
                    LikeCount = p.Likes.Count,
                    IsLiked = p.Likes.Any(l => l.MemberID == currentMemberId),

                    Comments = p.Comments
                        .OrderByDescending(c => c.CreatedAt)
                        .Take(5)
                        .Select(c => new
                        {
                            UserName = c.Member.FullName,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            AvatarURL = c.Member.AvatarURL
                        }).ToList(),

                    CommentTotal = p.Comments.Count
                })
                .FirstOrDefault();

            if (data == null) return null;

            return new ImageDetailViewModel
            {
                PictureID = data.PictureID,
                Title = data.Title,
                ImageURL = data.ImageURL,
                Description = data.Description,
                CategoryName = data.CategoryName,
                AuthorName = data.AuthorName,
                CreatedAt = data.CreatedAt,
                LikeCount = data.LikeCount,
                IsLiked = data.IsLiked,

                Comments = data.Comments.Select(c => new CommentViewModel
                {
                    UserName = c.UserName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    AvatarUrl = NormalizeAvatar(c.AvatarURL) // ✅ chạy ở memory
                }).ToList(),

                CommentTotal = data.CommentTotal
            };
        }

        public List<CommentViewModel> GetComments(int pictureId, int page = 1, int pageSize = 5)
        {
            var data = _context.Comments
                .Where(c => c.PictureID == pictureId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    UserName = c.Member.FullName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    AvatarURL = c.Member.AvatarURL
                })
                .ToList();

            return data.Select(c => new CommentViewModel
            {
                UserName = c.UserName,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AvatarUrl = NormalizeAvatar(c.AvatarURL)
            }).ToList();
        }

        public int GetCommentCount(int pictureId)
        {
            return _context.Comments.Count(c => c.PictureID == pictureId);
        }

        public Picture? GetById(int id)
        {
            return _context.Pictures
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.PictureID == id);
        }

        public bool ToggleLike(int pictureId, int memberId)
        {
            var existingLike = _context.Likes
                .FirstOrDefault(l => l.PictureID == pictureId && l.MemberID == memberId);

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
                _context.SaveChanges();
                return false; // vừa unlike
            }
            else
            {
                var like = new Like
                {
                    PictureID = pictureId,
                    MemberID = memberId
                };
                _context.Likes.Add(like);
                _context.SaveChanges();
                return true; // vừa like
            }
        }
        public int GetLikeCount(int pictureId)
        {
            return _context.Likes.Count(l => l.PictureID == pictureId);
        }

        public Comment? AddComment(int pictureId, int memberId, string content)
        {
            var picture = _context.Pictures.FirstOrDefault(p => p.PictureID == pictureId);
            var member = _context.Members.FirstOrDefault(m => m.MemberID == memberId);

            if (picture == null || member == null || string.IsNullOrWhiteSpace(content))
                return null;

            var comment = new Comment
            {
                PictureID = pictureId,
                MemberID = memberId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            // load đầy đủ thông tin member để frontend render
            comment.Member = member;

            return comment;
        }

        public void Add(Picture picture)
        {
            _context.Pictures.Add(picture);
        }

        public void Update(Picture picture)
        {
            _context.Pictures.Update(picture);
        }

        public void Delete(Picture picture)
        {
            _context.Pictures.Remove(picture);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}