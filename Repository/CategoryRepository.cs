using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;

namespace SharingPictureWebsite.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }
    }
}