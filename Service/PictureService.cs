using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;
using SharingPictureWebsite.Services.Interfaces;
using SharingPictureWebsite.ViewModels;
namespace SharingPictureWebsite.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly IAlbumPictureRepository _albumPictureRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IAlbumRepository _albumRepo;

        public PictureService(
            IPictureRepository repo,
            IAlbumPictureRepository albumPictureRepo,
            IWebHostEnvironment env,
            ICategoryRepository categoryRepo,
            IAlbumRepository albumRepo)
        {
            _repo = repo;
            _albumPictureRepo = albumPictureRepo;
            _env = env;
            _albumRepo = albumRepo;
            _categoryRepo = categoryRepo;
        }

        public UploadViewModel GetUploadData()
        {
            return new UploadViewModel
            {
                Categories = _categoryRepo.GetAll(),
                Albums = _albumRepo.GetAll()
            };
        }

        public async Task UploadImageAsync(
            IFormFile file,
            string title,
            string? description,
            int categoryId,
            int memberId,
            int? albumId // 🔥 sửa nullable
        )
        {
            // 1. Tạo folder
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // 2. Tạo file name
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            // 3. Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 4. Save Picture
            var picture = new Picture
            {
                Title = title,
                Description = description,
                ImageURL = "/uploads/" + fileName,
                FileSize = file.Length,
                CategoryID = categoryId,
                MemberID = memberId,
                Status = Status.Pending
            };

            _repo.Add(picture);
            _repo.Save();

            // 🔥 5. GẮN VÀO ALBUM (nếu có)
            if (albumId.HasValue && albumId.Value > 0)
            {
                var albumPicture = new AlbumPicture
                {
                    AlbumID = albumId.Value,
                    PictureID = picture.PictureID
                };

                _albumPictureRepo.Add(albumPicture);
                _albumPictureRepo.Save();
            }
        }
    }
}