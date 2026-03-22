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

        // Comments
        public List<CommentViewModel> Comments { get; set; } = new();

        // Phân trang comment
        public int CommentPage { get; set; } = 1;
        public int CommentPageSize { get; set; } = 5;
        public int CommentTotal { get; set; } = 0;
    }

    public class CommentViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string AvatarUrl { get; set; } = "/images/user/default-avatar.jpg"; 
    }
}