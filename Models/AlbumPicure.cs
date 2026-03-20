using SharingPictureWebsite.Models;

public class AlbumPicture
{
    public int AlbumID { get; set; }
    public virtual Album Album { get; set; } = null!;

    public int PictureID { get; set; }
    public virtual Picture Picture { get; set; } = null!;
}