using SharingPictureWebsite.Models;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<Member> GetAllMembers();

        AdminDashboardViewModel GetAdminDashboard(int page, int pageSize);

        DashboardStatsViewModel GetDashboardStats();

        (bool Success, string? ErrorMessage) Register(RegisterRequestViewModel request);

        void BanMember(int memberId);

        void UnbanMember(int memberId);
    }
}
