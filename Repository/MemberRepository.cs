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
            return _context.Members.FirstOrDefault(m => m.Email == email);
        }

        public Member? GetByUsername(string username)
        {
            return _context.Members
                .Include(m => m.Role)
                .FirstOrDefault(m => m.MemberName == username);
        }

        // Thêm login check
        public Member? ValidateLogin(string emailOrUsername, string password)
        {
            return _context.Members
                .Include(m => m.Role)
                .FirstOrDefault(m =>
                    (m.Email == emailOrUsername || m.MemberName == emailOrUsername) &&
                    m.Password == password &&
                    m.Status == Status.Active
                );
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