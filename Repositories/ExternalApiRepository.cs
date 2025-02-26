using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExternalApiBackend.Repositories
{
    public class ExternalApiRepository : IExternalApiRepository
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://reqres.in/api";

        public ExternalApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ✅ GET users
        public async Task<string> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/users");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // ✅ POST: Create user
        public async Task<string> CreateUserAsync(dynamic user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/users", jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // ✅ PUT: Update user
        public async Task<string> UpdateUserAsync(int id, dynamic user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/users/{id}", jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // ✅ DELETE: Remove user
        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/users/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
