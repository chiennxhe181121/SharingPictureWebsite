using SharingPictureWebsite.Models;

public class Role
{
    public int RoleID { get; set; }

    public string RoleName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Status Status { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}