using System.Threading.Tasks;

namespace ExternalApiBackend.Services
{
    public interface IExternalApiService
    {
        Task<string> GetUsersAsync();
        Task<string> CreateUserAsync(dynamic user);
        Task<string> UpdateUserAsync(int id, dynamic user);
        Task<bool> DeleteUserAsync(int id);
    }
}
