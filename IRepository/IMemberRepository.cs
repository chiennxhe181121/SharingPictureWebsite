using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
        Member? GetByEmail(string email);
        Member? GetByUsername(string username);
        Member? ValidateLogin(string emailOrUsername, string password);
        Member? GetByMemberName(string memberName);
        int? GetRoleIdByName(string roleName);

        void Add(Member member);
        void Update(Member member);
        void Delete(Member member);

        void Ban(int id);
        void Unban(int id);

        int Count();
        void Save();
    }
}
