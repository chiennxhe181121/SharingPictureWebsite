using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories
{
    public class CommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Comment> GetByPictureId(int pictureId)
        {
            return _context.Comments
                .Include(c => c.Member)
                .Where(c => c.PictureID == pictureId)
                .ToList();
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public int Count()
        {
            return _context.Comments.Count();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}