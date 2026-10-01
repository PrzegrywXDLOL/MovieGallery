using MovieGallery.Models;

namespace MovieGallery.Services.Interfaces
{
    public interface IUserDBService
    {
        Task<UserData> Get(string id);
        Task<IEnumerable<UserData>> GetAll();
        Task UpdateRole(UserData userData);
    }
}
