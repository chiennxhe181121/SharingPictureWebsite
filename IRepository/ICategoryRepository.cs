using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
    }
}