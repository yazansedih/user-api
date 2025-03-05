using ExternalApiBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExternalApiBackend.Repositories
{
    public interface IMyRepository
    {
        // ------------------------------
        // LOCAL JSON USER METHODS
        // ------------------------------
        Task<List<User>> GetLocalUsersAsync();
        Task<User> GetLocalUserByIdAsync(int id);
        Task<User> CreateLocalUserAsync(User user);
        Task<User> UpdateLocalUserAsync(int id, User user);
        Task<bool> DeleteLocalUserAsync(int id);

        // ------------------------------
        // EXTERNAL API METHODS
        // ------------------------------
        Task<string> GetExternalUsersAsync();
        Task<string> CreateExternalUserAsync(dynamic user);
        Task<string> UpdateExternalUserAsync(int id, dynamic user);
        Task<bool> DeleteExternalUserAsync(int id);
    }
}
