using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;
using SharingPictureWebsite.Services.Interfaces;
using SharingPictureWebsite.ViewModels;

namespace SharingPictureWebsite.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IPictureRepository _pictureRepository;

        public MemberService(IMemberRepository memberRepository, IPictureRepository pictureRepository)
        {
            _memberRepository = memberRepository;
            _pictureRepository = pictureRepository;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _memberRepository.GetAll();
        }

        public DashboardStatsViewModel GetDashboardStats()
        {
            var members = _memberRepository.GetAll().ToList();
            var pictures = _pictureRepository.GetAll().ToList();

            var totalUsers = members.Count;
            var totalImages = pictures.Count;
            var activeUsers = members.Count(m => m.Status == Status.Active);
            var bannedUsers = members.Count(m => m.Status == Status.Banned);
            var approvedImages = pictures.Count(p => p.Status == Status.Public);

            return new DashboardStatsViewModel
            {
                TotalUsers = totalUsers,
                TotalImages = totalImages,
                ActiveUsers = activeUsers,
                BannedUsers = bannedUsers,
                ApprovedImages = approvedImages,
                ActivePercent = totalUsers == 0 ? 0 : (activeUsers * 100) / totalUsers,
                BannedPercent = totalUsers == 0 ? 0 : (bannedUsers * 100) / totalUsers
            };
        }

        public void BanMember(int memberId)
        {
            _memberRepository.Ban(memberId);
            _memberRepository.Save();
        }

        public void UnbanMember(int memberId)
        {
            _memberRepository.Unban(memberId);
            _memberRepository.Save();
        }
    }
}
