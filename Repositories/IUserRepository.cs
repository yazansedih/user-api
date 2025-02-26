using ExternalApiBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExternalApiBackend.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
