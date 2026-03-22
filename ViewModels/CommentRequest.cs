namespace SharingPictureWebsite.ViewModels
{
    public class CommentRequest
    {
        public string UserName { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        // Thêm dòng này để View có thể đọc được đường dẫn ảnh
        public string? UserAvatar { get; set; }
    }
}
