using SharingPictureWebsite.Models;

public class Picture
{
    public int PictureID { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public string ImageURL { get; set; } = null!;

    public long FileSize { get; set; }

    public DateTime UploadDate { get; set; }

    public Status Status { get; set; }

    public int MemberID { get; set; }
    public virtual Member Author { get; set; } = null!;

    public int? CategoryID { get; set; }
    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<AlbumPicture> AlbumPictures { get; set; } = new List<AlbumPicture>();
}