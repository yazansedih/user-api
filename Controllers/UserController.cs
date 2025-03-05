using ExternalApiBackend.Models;
using ExternalApiBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Dtos.User;

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

        // GET all users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // GET user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // GET user full name by ID
        [HttpGet("{id}/fullname")]
        public async Task<ActionResult<FullNameDto>> GetUserFullName(int id)
        {
            // 1) Fetch the user
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            // 2) Build a DTO that has only the full name
            var dto = new FullNameDto
            {
                FullName = $"{user.FirstName} {user.LastName}"
            };

            // 3) Return the DTO
            return Ok(dto);
        }

        // POST: Create user
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Avatar = dto.Avatar
            };

            var newUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }

        // PUT: Update user
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] UserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Avatar = dto.Avatar
            };

            var updatedUser = await _userService.UpdateUserAsync(id, user);
            return updatedUser == null ? NotFound() : Ok(updatedUser);
        }

        // DELETE: Remove user
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            return success ? Ok("User deleted.") : NotFound();
        }
    }
}
