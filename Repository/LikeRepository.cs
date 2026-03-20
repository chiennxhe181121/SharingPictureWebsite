using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories
{
    public class LikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Like like)
        {
            _context.Likes.Add(like);
        }

        public void Remove(Like like)
        {
            _context.Likes.Remove(like);
        }

        public int CountByPicture(int pictureId)
        {
            return _context.Likes.Count(l => l.PictureID == pictureId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}