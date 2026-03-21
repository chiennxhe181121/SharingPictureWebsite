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

        public ImageDetailViewModel? GetPictureDetail(int pictureId, int currentMemberId)
        {
            return _context.Pictures
                .Where(p => p.PictureID == pictureId)
                .Select(p => new ImageDetailViewModel
                {
                    PictureID = p.PictureID,
                    Title = p.Title,
                    ImageURL = p.ImageURL,
                    Description = p.Description,
                    CategoryName = p.Category != null ? p.Category.CategoryName : "No Category",
                    AuthorName = p.Author.FullName,
                    CreatedAt = p.UploadDate,
                    LikeCount = p.Likes.Count,
                    IsLiked = p.Likes.Any(l => l.MemberID == currentMemberId), // check tạm member id 2
                    Comments = p.Comments
                                .OrderByDescending(c => c.CreatedAt)
                                .Select(c => new CommentViewModel
                                {
                                    UserName = c.Member.FullName,
                                    Content = c.Content,
                                    CreatedAt = c.CreatedAt
                                }).ToList()
                }).FirstOrDefault();
        }

        public Picture? GetById(int id)
        {
            return _context.Pictures
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.PictureID == id);
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