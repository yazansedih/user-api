using ExternalApiBackend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExternalApiBackend.Repositories
{
    public class MyRepository : IMyRepository
    {
        // Local JSON storage
        private readonly string _filePath;

        // External API
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://reqres.in/api";

        public MyRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Initialize local JSON file storage
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "data");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _filePath = Path.Combine(directory, "users.json");
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "{ \"users\": [] }");
            }
        }

        // ----------------------------------------------------
        // LOCAL JSON METHODS
        // ----------------------------------------------------
        public async Task<List<User>> GetLocalUsersAsync()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<User>();

            try
            {
                var data = JsonSerializer.Deserialize<UserData>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return data?.Users ?? new List<User>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<User> GetLocalUserByIdAsync(int id)
        {
            var users = await GetLocalUsersAsync();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> CreateLocalUserAsync(User user)
        {
            var users = await GetLocalUsersAsync();
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            await SaveLocalUsersAsync(users);
            return user;
        }

        public async Task<User> UpdateLocalUserAsync(int id, User updatedUser)
        {
            var users = await GetLocalUsersAsync();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null) return null;

            // Only update fields if they're not null
            user.Email = updatedUser.Email ?? user.Email;
            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;
            user.Avatar = updatedUser.Avatar ?? user.Avatar;

            await SaveLocalUsersAsync(users);
            return user;
        }

        public async Task<bool> DeleteLocalUserAsync(int id)
        {
            var users = await GetLocalUsersAsync();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null) return false;

            users.Remove(user);
            await SaveLocalUsersAsync(users);
            return true;
        }

        // ----------------------------------------------------
        // EXTERNAL API METHODS
        // ----------------------------------------------------
        public async Task<string> GetExternalUsersAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/users");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateExternalUserAsync(dynamic user)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync($"{BaseUrl}/users", jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateExternalUserAsync(int id, dynamic user)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PutAsync($"{BaseUrl}/users/{id}", jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> DeleteExternalUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/users/{id}");
            return response.IsSuccessStatusCode;
        }

        // ----------------------------------------------------
        // PRIVATE HELPER FOR SAVING LOCAL USERS TO JSON FILE
        // ----------------------------------------------------
        private async Task SaveLocalUsersAsync(List<User> users)
        {
            var json = JsonSerializer.Serialize(
                new { users },
                new JsonSerializerOptions { WriteIndented = true }
            );
            await File.WriteAllTextAsync(_filePath, json);
        }

        // ----------------------------------------------------
        // NESTED CLASS FOR DESERIALIZATION
        // ----------------------------------------------------
        private class UserData
        {
            public List<User> Users { get; set; } = new List<User>();
        }
    }
}
