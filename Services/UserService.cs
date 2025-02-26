using ExternalApiBackend.Models;
using ExternalApiBackend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExternalApiBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetUsersAsync() => await _userRepository.GetUsersAsync();
        public async Task<User> GetUserByIdAsync(int id) => await _userRepository.GetUserByIdAsync(id);
        public async Task<User> CreateUserAsync(User user) => await _userRepository.CreateUserAsync(user);
        public async Task<User> UpdateUserAsync(int id, User user) => await _userRepository.UpdateUserAsync(id, user);
        public async Task<bool> DeleteUserAsync(int id) => await _userRepository.DeleteUserAsync(id);
    }
}
