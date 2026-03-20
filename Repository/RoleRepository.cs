using SharingPictureWebsite.Data;
using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories
{
    public class RoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.ToList();
        }

        public Role? GetById(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.RoleID == id);
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }

        public void Delete(Role role)
        {
            _context.Roles.Remove(role);
        }

        public int Count()
        {
            return _context.Roles.Count();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}