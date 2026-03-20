using SharingPictureWebsite.Models;

public class Member
{
    public int MemberID { get; set; }

    public int RoleID { get; set; }
    public virtual Role Role { get; set; } = null!;

    public string MemberName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string? FullName { get; set; }
    public string? Bio { get; set; }
    public string? AvatarURL { get; set; }

    public DateTime CreatedAt { get; set; }

    public Status Status { get; set; }

    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}