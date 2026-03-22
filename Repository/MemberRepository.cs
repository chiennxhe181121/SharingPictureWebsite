using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;
using SharingPictureWebsite.Repositories.Interfaces;

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