using SharingPictureWebsite.Models;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Repositories.Interfaces
{
    public interface IPictureRepository
    {
        (List<PictureDTO>, int totalItems) GetPublicPictures(
    string? search,
    int? categoryId,
    string? sortBy,
    int page,
    int pageSize);

        IEnumerable<Picture> GetAll();
        ImageDetailViewModel? GetPictureDetail(int pictureId, int currentMemberId);
        Picture? GetById(int id);

        void Add(Picture picture);
        void Update(Picture picture);
        void Delete(Picture picture);

        void Save();
    }
}