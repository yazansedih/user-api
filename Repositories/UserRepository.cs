using ExternalApiBackend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExternalApiBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _filePath;

        public UserRepository()
        {
            // ✅ Ensure `data/` folder exists
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "data");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _filePath = Path.Combine(directory, "users.json");

            // ✅ Initialize empty file if not exists
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "{ \"users\": [] }");
            }
        }

        // ✅ Get all users
        public async Task<List<User>> GetUsersAsync()
        {
            if (!File.Exists(_filePath)) return new List<User>();

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<User>();

            try
            {
                var data = JsonSerializer.Deserialize<UserData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data?.Users ?? new List<User>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                return new List<User>(); // ✅ Handle JSON errors gracefully
            }
        }

        // ✅ Get a single user by ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            var users = await GetUsersAsync();
            return users.FirstOrDefault(u => u.Id == id);
        }

        // ✅ Create a new user (Append without deleting old ones)
        public async Task<User> CreateUserAsync(User user)
        {
            var users = await GetUsersAsync();
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1; // ✅ Assign a unique ID
            users.Add(user);
            await SaveUsersAsync(users);
            return user;
        }

        // ✅ Update an existing user
        public async Task<User> UpdateUserAsync(int id, User updatedUser)
        {
            var users = await GetUsersAsync();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null) return null; // ✅ Return `null` if user is not found

            // ✅ Update user fields if they are not null
            user.Email = updatedUser.Email ?? user.Email;
            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;
            user.Avatar = updatedUser.Avatar ?? user.Avatar;

            await SaveUsersAsync(users);
            return user;
        }

        // ✅ Delete a user
        public async Task<bool> DeleteUserAsync(int id)
        {
            var users = await GetUsersAsync();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null) return false; // ✅ Return `false` if user is not found

            users.Remove(user);
            await SaveUsersAsync(users);
            return true;
        }

        // ✅ Save users back to `users.json`
        private async Task SaveUsersAsync(List<User> users)
        {
            var json = JsonSerializer.Serialize(new { users }, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }

        // ✅ Helper class for JSON mapping
        private class UserData
        {
            public List<User> Users { get; set; } = new List<User>();
        }
    }
}
