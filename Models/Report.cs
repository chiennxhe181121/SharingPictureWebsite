using SharingPictureWebsite.Models;

public class Report
{
    public int ReportID { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Status Status { get; set; }

    public int MemberID { get; set; }
    public virtual Member Reporter { get; set; } = null!;

    public int PictureID { get; set; }
    public virtual Picture Picture { get; set; } = null!;
}