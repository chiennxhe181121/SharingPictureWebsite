using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;
using SharingPictureWebsite.Service.Security;

namespace SharingPictureWebsite.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Member> GetAll()
        {
            return _context.Members
                .Include(m => m.Role)
                .OrderBy(m => m.MemberName)
                .ToList();
        }

        public Member? GetById(int id)
        {
            return _context.Members
                .Include(m => m.Role)
                .FirstOrDefault(m => m.MemberID == id);
        }

        public Member? GetByEmail(string email)
        {
            var normalizedEmail = email.Trim().ToLower();
            return _context.Members.FirstOrDefault(m => m.Email.ToLower() == normalizedEmail);
        }

        public Member? GetByMemberName(string memberName)
        {
            var normalizedMemberName = memberName.Trim().ToLower();
            return _context.Members.FirstOrDefault(m => m.MemberName.ToLower() == normalizedMemberName);
        }

        public int? GetRoleIdByName(string roleName)
        {
            var normalizedRoleName = roleName.Trim().ToLower();
            return _context.Roles
                .Where(r => r.RoleName.ToLower() == normalizedRoleName)
                .Select(r => (int?)r.RoleID)
                .FirstOrDefault();
        }

        public Member? GetByUsername(string username)
        {
            return _context.Members
                .Include(m => m.Role)
                .FirstOrDefault(m => m.MemberName == username);
        }

        public Member? ValidateLogin(string emailOrUsername, string password)
        {
            var user = _context.Members
                .Include(m => m.Role)
                .FirstOrDefault(m =>
                    (m.Email == emailOrUsername || m.MemberName == emailOrUsername) &&
                    m.Status == Status.Active
                );

            if (user == null)
                return null;

            // Check password hash
            if (!AppPasswordHasher.VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public void Add(Member member)
        {
            _context.Members.Add(member);
        }

        public void Update(Member member)
        {
            _context.Members.Update(member);
        }

        public void Delete(Member member)
        {
            _context.Members.Remove(member);
        }

        public void Ban(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                user.Status = Status.Banned;
                Update(user);
            }
        }

        public void Unban(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                user.Status = Status.Active;
                Update(user);
            }
        }
        
        public void UpdateMemberRole(int memberId, int roleId)
        {
            var member = GetById(memberId);
            if (member != null)
            {
                member.RoleID = roleId;
                Update(member);
            }
        }
        
        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Roles.Where(r => r.Status == Status.Active).ToList();
        }

        public int Count()
        {
            return _context.Members.Count();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}