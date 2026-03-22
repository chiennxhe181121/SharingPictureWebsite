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
        List<CommentViewModel> GetComments(int pictureId, int page = 1, int pageSize = 5);
        int GetCommentCount(int pictureId);
        Picture? GetById(int id);
        bool ToggleLike(int pictureId, int memberId);
        int GetLikeCount(int pictureId);
        Comment? AddComment(int pictureId, int memberId, string content);

        void Add(Picture picture);
        void Update(Picture picture);
        void Delete(Picture picture);

        void Save();
    }
}