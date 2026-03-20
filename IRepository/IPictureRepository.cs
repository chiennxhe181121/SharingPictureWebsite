using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories.Interfaces
{
    public interface IPictureRepository
    {
        IEnumerable<Picture> GetAll();
        Picture? GetById(int id);

        void Add(Picture picture);
        void Update(Picture picture);
        void Delete(Picture picture);

        void Save();
    }
}