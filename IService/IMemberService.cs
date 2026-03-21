using SharingPictureWebsite.Models;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<Member> GetAllMembers();

        DashboardStatsViewModel GetDashboardStats();

        void BanMember(int memberId);

        void UnbanMember(int memberId);
    }
}
