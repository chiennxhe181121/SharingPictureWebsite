using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.ViewModels
{
    public class GalleryViewModel
    {
        public List<PictureDTO> Pictures { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; }

        public List<Category> Categories { get; set; } = new();
    }
}
