using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;

namespace SharingPictureWebsite.Repositories
{
    public class AlbumPictureRepository : IAlbumPictureRepository
    {
        private readonly AppDbContext _context;

        public AlbumPictureRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(AlbumPicture entity)
        {
            _context.AlbumPictures.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}