namespace SharingPictureWebsite.ViewModels
{
    public class ImageDetailViewModel
    {
        public int PictureID { get; set; }
        public string Title { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public string Description { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
    }

    public class CommentViewModel
    {
        public string UserName { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}