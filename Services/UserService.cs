using ExternalApiBackend.Models;
using ExternalApiBackend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExternalApiBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IMyRepository _myRepository;

        public UserService(IMyRepository myRepository)
        {
            _myRepository = myRepository;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _myRepository.GetLocalUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _myRepository.GetLocalUserByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await _myRepository.CreateLocalUserAsync(user);
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            return await _myRepository.UpdateLocalUserAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _myRepository.DeleteLocalUserAsync(id);
        }
    }
}
