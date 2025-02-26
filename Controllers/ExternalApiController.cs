namespace ExternalApiBackend.Controllers
{
    using ExternalApiBackend.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/external")]
    [ApiController]
    public class ExternalApiController : ControllerBase
    {
        private readonly IExternalApiService _externalApiService;

        public ExternalApiController(IExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        // ✅ GET users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var data = await _externalApiService.GetUsersAsync();
            return Ok(data);
        }

        // ✅ POST: Create user
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] dynamic user)
        {
            var data = await _externalApiService.CreateUserAsync(user);
            return Ok(data);
        }

        // ✅ PUT: Update user
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] dynamic user)
        {
            var data = await _externalApiService.UpdateUserAsync(id, user);
            return Ok(data);
        }

        // ✅ DELETE: Remove user
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _externalApiService.DeleteUserAsync(id);
            return result ? Ok("User deleted successfully.") : BadRequest("Failed to delete user.");
        }
    }
}
