using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories.Interfaces
{
    public interface IAlbumPictureRepository
    {
        void Add(AlbumPicture entity);
        void Save();
    }
}