using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        List<Album> GetAll();
    }
}