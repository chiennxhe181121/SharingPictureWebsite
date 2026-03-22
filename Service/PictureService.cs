using Microsoft.EntityFrameworkCore;
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

        public GalleryViewModel GetPublicGallery(
            string? search,
            int? categoryId,
            string? sortBy,
            int page,
            int pageSize)
        {
            var (pictures, totalItems) = _repo.GetPublicPictures(
                search, categoryId, sortBy, page, pageSize);

            return new GalleryViewModel
            {
                Pictures = pictures,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Search = search,
                CategoryId = categoryId,
                SortBy = sortBy,
                Categories = _categoryRepo.GetAll()
            };
        }

        public ImageDetailViewModel? GetPictureDetail(int pictureId, int currentMemberId)
        {
            return _repo.GetPictureDetail(pictureId, currentMemberId);
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
            int? albumId
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

        public IEnumerable<Picture> GetAllPictures()
        {
            return _repo.GetAll();
        }

        public IEnumerable<Picture> GetModeratorPictures()
        {
            return _repo
                .GetAll()
                .OrderByDescending(p => p.UploadDate)
                .ToList();
        }

        public IEnumerable<Picture> GetModeratorPicturesByStatus(string? status)
        {
            var pictures = _repo.GetAll().OrderByDescending(p => p.UploadDate);

            if (string.IsNullOrEmpty(status) || status.ToLower() == "all")
            {
                return pictures.ToList();
            }

            return status.ToLower() switch
            {
                "pending" => pictures.Where(p => p.Status == Status.Pending).ToList(),
                "approved" => pictures.Where(p => p.Status == Status.Public).ToList(),
                "rejected" => pictures.Where(p => p.Status == Status.Rejected || p.Status == Status.Banned).ToList(),
                _ => pictures.ToList()
            };
        }

        public ModeratorStatusStatsViewModel GetModeratorStatusStats()
        {
            var pictures = _repo.GetAll().ToList();

            return new ModeratorStatusStatsViewModel
            {
                PendingCount = pictures.Count(p => p.Status == Status.Pending),
                ApprovedCount = pictures.Count(p => p.Status == Status.Public),
                RejectedCount = pictures.Count(p => p.Status == Status.Rejected || p.Status == Status.Banned)
            };
        }

        public ModeratorDashboardViewModel GetModeratorDashboard(string? status, int page = 1, int pageSize = 5)
        {
            return new ModeratorDashboardViewModel
            {
                Pictures = GetModeratorPicturesPaged(status, page, pageSize),
                Stats = GetModeratorStatusStats(),
                CurrentFilter = string.IsNullOrWhiteSpace(status) ? "all" : status.ToLower()
            };
        }

        public PaginationViewModel<Picture> GetModeratorPicturesPaged(string? status, int page = 1, int pageSize = 5)
        {
            var allPictures = GetModeratorPicturesByStatus(status).ToList();
            var totalItems = allPictures.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Validate page number
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var itemsToSkip = (page - 1) * pageSize;
            var itemsOnPage = allPictures.Skip(itemsToSkip).Take(pageSize).ToList();

            return new PaginationViewModel<Picture>
            {
                Items = itemsOnPage,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize
            };
        }

        public void ApprovePicture(int pictureId)
        {
            var picture = _repo.GetById(pictureId);
            if (picture != null)
            {
                picture.Status = Status.Public;
                _repo.Update(picture);
                _repo.Save();
            }
        }

        public void RejectPicture(int pictureId)
        {
            var picture = _repo.GetById(pictureId);
            if (picture != null)
            {
                picture.Status = Status.Rejected;
                _repo.Update(picture);
                _repo.Save();
            }
        }
    }
}