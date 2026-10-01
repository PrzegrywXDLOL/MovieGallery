using MovieGallery.Models;
using MovieGallery.Services.Interfaces;

namespace MovieGallery.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDBService _db;
        public UserService(IUserDBService db)
        {
            _db = db;
        }

        public async Task<UserData> Get(string id)
        {
            return await _db.Get(id);
        }

        public async Task<IEnumerable<UserData>> GetAll()
        {
            return await _db.GetAll();
        }

        public async Task UpdateRole(UserData userData)
        {
            await _db.UpdateRole(userData);
        }
    }
}
