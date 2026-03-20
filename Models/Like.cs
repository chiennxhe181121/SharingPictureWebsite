using SharingPictureWebsite.Models;

public class Like
{
    public int LikeID { get; set; }

    public int MemberID { get; set; }
    public virtual Member Member { get; set; } = null!;

    public int PictureID { get; set; }
    public virtual Picture Picture { get; set; } = null!;
}