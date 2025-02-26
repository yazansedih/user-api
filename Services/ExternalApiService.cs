using ExternalApiBackend.Repositories;
using System.Threading.Tasks;

namespace ExternalApiBackend.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly IExternalApiRepository _externalApiRepository;

        public ExternalApiService(IExternalApiRepository externalApiRepository)
        {
            _externalApiRepository = externalApiRepository;
        }

        public async Task<string> GetUsersAsync()
        {
            return await _externalApiRepository.GetUsersAsync();
        }

        public async Task<string> CreateUserAsync(dynamic user)
        {
            return await _externalApiRepository.CreateUserAsync(user);
        }

        public async Task<string> UpdateUserAsync(int id, dynamic user)
        {
            return await _externalApiRepository.UpdateUserAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _externalApiRepository.DeleteUserAsync(id);
        }
    }
}
