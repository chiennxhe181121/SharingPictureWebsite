using SharingPictureWebsite.Models;

public class Category
{
    public int CategoryID { get; set; }

    public string CategoryName { get; set; } = null!;

    public Status Status { get; set; }

    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
}