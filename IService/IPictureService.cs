using Microsoft.AspNetCore.Http;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Services.Interfaces
{
    public interface IPictureService
    {
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

        void ApprovePicture(int pictureId);

        void RejectPicture(int pictureId);
    }
}