using Microsoft.EntityFrameworkCore;
using MovieGallery.Data;
using MovieGallery.Models;
using MovieGallery.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MovieGallery.Services
{
    public class UserDBService: IUserDBService
    {
        private readonly MovieGalleryDBContext _db;

        public UserDBService(MovieGalleryDBContext db)
        {
            _db = db;
        }
        public async Task<UserData?> Get(string id)
        {
            var user = await _db.Users
                .Where(user => user.Id == id)
                .Select(user => new UserData
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty
                })
                .FirstOrDefaultAsync();

            if (user is null) return null;

            user.RoleId = await _db.UserRoles
                .Where(userRole => userRole.UserId == id)
                .Select(userRole => userRole.RoleId)
                .FirstOrDefaultAsync() ?? string.Empty;

            return user;
        }

        public async Task<IEnumerable<UserData>> GetAll()
        {
            return await _db.Users
                .OrderBy(user => user.UserName)
                .Select(user => new UserData
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    RoleId = _db.UserRoles
                        .Where(userRole => userRole.UserId == user.Id)
                        .Select(userRole => userRole.RoleId)
                        .FirstOrDefault() ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task UpdateRole(UserData userData)
        {
            var roleId = userData.RoleId?.Trim() ?? string.Empty;

            var currentRole = await _db.UserRoles
                .SingleOrDefaultAsync(userRole => userRole.UserId == userData.Id);

            if (currentRole is not null) _db.UserRoles.Remove(currentRole);

            if (!string.IsNullOrEmpty(roleId))
            {
                _db.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = userData.Id,
                    RoleId = roleId
                });
            }

            await _db.SaveChangesAsync();
        }
    }
}
