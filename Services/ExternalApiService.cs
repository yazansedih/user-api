using ExternalApiBackend.Repositories;
using System.Threading.Tasks;

namespace ExternalApiBackend.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly IMyRepository _myRepository;

        public ExternalApiService(IMyRepository myRepository)
        {
            _myRepository = myRepository;
        }

        public async Task<string> GetUsersAsync()
        {
            return await _myRepository.GetExternalUsersAsync();
        }

        public async Task<string> CreateUserAsync(dynamic user)
        {
            return await _myRepository.CreateExternalUserAsync(user);
        }

        public async Task<string> UpdateUserAsync(int id, dynamic user)
        {
            return await _myRepository.UpdateExternalUserAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _myRepository.DeleteExternalUserAsync(id);
        }
    }
}
