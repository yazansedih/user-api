using ExternalApiBackend.Models;
using ExternalApiBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExternalApiBackend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ GET all users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        // ✅ GET user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // ✅ POST: Create user
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var newUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }

        // ✅ PUT: Update user
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, user);
            return updatedUser == null ? NotFound() : Ok(updatedUser);
        }

        // ✅ DELETE: Remove user
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            return await _userService.DeleteUserAsync(id) ? Ok("User deleted.") : NotFound();
        }
    }
}
