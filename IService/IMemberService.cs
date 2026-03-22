using SharingPictureWebsite.Models;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Services.Interfaces
{
    public interface IMemberService
    {
        Member? Login(string emailOrUsername, string password);
        bool IsAdmin(Member member);
        bool IsModerator(Member member);
        bool IsMember(Member member);

        IEnumerable<Member> GetAllMembers();

        AdminDashboardViewModel GetAdminDashboard(int page, int pageSize);

        DashboardStatsViewModel GetDashboardStats();

        void BanMember(int memberId);

        void UnbanMember(int memberId);
    }
}
