using System.Net.Mail;
using System.Text.RegularExpressions;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;
using SharingPictureWebsite.Service.Security;
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
        public Member? Login(string emailOrUsername, string password)
        {
            // Trường hợp sau này có bcrypt, replace password check ở đây
            return _memberRepository.ValidateLogin(emailOrUsername, password);
        }

        public bool IsAdmin(Member member) => member.Role.RoleName == "Admin";
        public bool IsModerator(Member member) => member.Role.RoleName == "Moderator";
        public bool IsMember(Member member) => member.Role.RoleName == "Member";

        public IEnumerable<Member> GetAllMembers()
        {
            return _memberRepository.GetAll();
        }

        public AdminDashboardViewModel GetAdminDashboard(int page, int pageSize)
        {
            var members = _memberRepository.GetAll().ToList();
            var totalItems = members.Count;
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
            page = Math.Max(1, Math.Min(page, totalPages));

            var pagedMembers = members
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new AdminDashboardViewModel
            {
                Stats = GetDashboardStats(),
                Members = new PaginationViewModel<Member>
                {
                    Items = pagedMembers,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    PageSize = pageSize
                }
            };
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

        public (bool Success, string? ErrorMessage) Register(RegisterRequestViewModel request)
        {
            var memberName = request.MemberName.Trim();
            var fullName = request.FullName.Trim();
            var email = request.Email.Trim();

            if (string.IsNullOrWhiteSpace(memberName) ||
                string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return (false, "Please fill in all required fields.");
            }

            if (memberName.Any(char.IsWhiteSpace))
            {
                return (false, "Member name cannot contain spaces.");
            }

            if (!IsValidEmail(email))
            {
                return (false, "Invalid email format.");
            }

            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match.");
            }

            if (!IsStrongPassword(request.Password))
            {
                return (false, "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.");
            }

            if (_memberRepository.GetByMemberName(memberName) != null)
            {
                return (false, "Member name already exists.");
            }

            if (_memberRepository.GetByEmail(email) != null)
            {
                return (false, "Email already exists.");
            }

            var memberRoleId = _memberRepository.GetRoleIdByName("Member");
            if (!memberRoleId.HasValue)
            {
                return (false, "Member role not found.");
            }

            var member = new Member
            {
                MemberName = memberName,
                FullName = fullName,
                Email = email,
                Password = AppPasswordHasher.HashPassword(request.Password),
                RoleID = memberRoleId.Value,
                Status = Status.Active,
                AvatarURL = "/images/default-avatar.jpg",
                CreatedAt = DateTime.Now
            };

            _memberRepository.Add(member);
            _memberRepository.Save();

            return (true, null);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var _ = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsStrongPassword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$");
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
