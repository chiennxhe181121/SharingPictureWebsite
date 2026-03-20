using SharingPictureWebsite.Models;

public class Comment
{
    public int CommentID { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int MemberID { get; set; }
    public virtual Member Member { get; set; } = null!;

    public int PictureID { get; set; }
    public virtual Picture Picture { get; set; } = null!;
}