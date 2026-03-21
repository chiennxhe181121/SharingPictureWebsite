namespace SharingPictureWebsite.ViewModels
{
    public class PictureDTO
    {
        public int PictureID { get; set; }
        public string Title { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public string? CategoryName { get; set; }
        public string AuthorName { get; set; } = null!;
        public DateTime UploadDate { get; set; }
    }
}