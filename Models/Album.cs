using SharingPictureWebsite.Models;

public class Album
{
    public int AlbumID { get; set; }

    public string AlbumName { get; set; } = null!;

    public string? Description { get; set; }

    public string PrivacyMode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int MemberID { get; set; }

    public virtual Member Owner { get; set; } = null!;

    public virtual ICollection<AlbumPicture> AlbumPictures { get; set; } = new List<AlbumPicture>();
}