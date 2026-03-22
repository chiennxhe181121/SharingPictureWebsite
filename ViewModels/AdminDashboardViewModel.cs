using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.ViewModels
{
    public class AdminDashboardViewModel
    {
        public DashboardStatsViewModel Stats { get; set; } = new();
        public PaginationViewModel<Member> Members { get; set; } = new();
        public IEnumerable<Role> Roles { get; set; } = new List<Role>();
    }
}