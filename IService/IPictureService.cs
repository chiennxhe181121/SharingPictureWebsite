using Microsoft.AspNetCore.Http;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Services.Interfaces
{
    public interface IPictureService
    {
        GalleryViewModel GetPublicGallery(
            string? search,
            int? categoryId,
            string? sortBy,
            int page,
            int pageSize);
        ImageDetailViewModel? GetPictureDetail(int pictureId, int currentMemberId);

        Task UploadImageAsync(
            IFormFile file,
            string title,
            string? description,
            int categoryId,
            int memberId,
            int? albumId
        );

        UploadViewModel GetUploadData();

        IEnumerable<Picture> GetAllPictures();

        IEnumerable<Picture> GetModeratorPictures();

        IEnumerable<Picture> GetModeratorPicturesByStatus(string? status);

        PaginationViewModel<Picture> GetModeratorPicturesPaged(string? status, int page = 1, int pageSize = 5);

        ModeratorDashboardViewModel GetModeratorDashboard(string? status, int page = 1, int pageSize = 5);

        ModeratorStatusStatsViewModel GetModeratorStatusStats();

        void ApprovePicture(int pictureId);

        void RejectPicture(int pictureId);
    }
}