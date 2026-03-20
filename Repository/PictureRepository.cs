using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Data;
using SharingPictureWebsite.Repositories.Interfaces;

namespace SharingPictureWebsite.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly AppDbContext _context;

        public PictureRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Picture> GetAll()
        {
            return _context.Pictures
                .Include(p => p.Author)
                .Include(p => p.Category)
                .ToList();
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