using Microsoft.AspNetCore.Mvc;
using ShiftTool.Server.Exceptions;
using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Helpers;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Models;
using System.Threading.Tasks;

namespace ShiftTool.Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(UserDTO userDto)
        {
            var existingUser = await _userRepository.GetCurrentUserAsync(userDto.Email);
            if (existingUser != null)
            {
                return BadRequest("En bruger med denne e-mail findes allerede.");
            }

            var user = ConvertFromDTO(userDto);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.CreateUserAsync(user);

            var createdUser = await _userRepository.GetCurrentUserAsync(user.Email);
            if (createdUser == null)
            {
                return StatusCode(500, "En fejl opstod under oprettelsen af brugeren.");
            }

            var createdUserDto = new UserDTO
            {
                UserId = createdUser.UserId,
                Email = createdUser.Email,
                FullName = createdUser.FullName,
                PhoneNumber = createdUser.PhoneNumber,
                IsCoordinator = createdUser.IsCoordinator,
                Experience = createdUser.Experience,
                Skills = createdUser.Skills,
                CreatedAt = createdUser.CreatedAt
            };

            return Ok(createdUserDto);
        }

        // Log en bruger ind
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserDTO userDto)
        {
            var user = await _userRepository.GetCurrentUserAsync(userDto.Email);

            if (user == null)
            {
                return Unauthorized(new { Success = false, Message = "Bruger findes ikke." });
            }

            if (userDto.Password != user.Password)
            {
                return Unauthorized(new { Success = false, Message = "Forkert password." });
            }

            var loginResponse = new LoginResponse
            {
                Success = true,
                UserData = new UserDTO
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Experience = user.Experience,
                    Skills = user.Skills,
                    IsCoordinator = user.IsCoordinator,
                }
            };

            return Ok(loginResponse);
        }

        // Log en bruger ud
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                // Ryd sessionen eller tokenet her
                // Fjern nødvendige cookies eller sessiondata
                HttpContext.Session.Clear();

                return Ok(new { Message = "Logget ud succesfuldt." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Fejl ved logout: {ex.Message}" });
            }
        }

        // Opret en bruger
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync(UserDTO userDto)
        {
            var user = ConvertFromDTO(userDto);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.CreateUserAsync(user);
            return Ok("Bruger oprettet succesfuldt");
        }

        // Hent alle brugere
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        // Hent en bruger ved e-mail
        [HttpGet("get-user/{email}")]
        public async Task<IActionResult> GetCurrentUserAsync(string email)
        {
            var user = await _userRepository.GetCurrentUserAsync(email);
            return user != null ? Ok(user) : NotFound("Bruger blev ikke fundet");
        }

        [HttpPut("update-user/{email}")]
        public async Task<IActionResult> UpdateUserAsync(string email, UserDTO userDto)
        {
            try
            {
                var existingUser = await _userRepository.GetCurrentUserAsync(email);
                if (existingUser == null) return NotFound("Bruger blev ikke fundet");

                existingUser.FullName = userDto.FullName;
                existingUser.PhoneNumber = userDto.PhoneNumber;
                existingUser.IsCoordinator = userDto.IsCoordinator;
                existingUser.Experience = userDto.Experience;
                existingUser.Skills = userDto.Skills;

                if (!string.IsNullOrEmpty(userDto.Password))
                {
                    existingUser.Password = userDto.Password; 
                }

                await _userRepository.UpdateUserAsync(existingUser);
                return Ok("Bruger opdateret succesfuldt");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Der opstod en intern serverfejl under opdateringen af brugeren");
            }
        }


        // Slet en bruger
        [HttpDelete("delete-user/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _userRepository.DeleteUserAsync(email);
            return Ok("Bruger slettet succesfuldt");
        }

        // Konverter fra UserDTO til User model
        private User ConvertFromDTO(UserDTO userDto)
        {
            return new User
            {
                Email = userDto.Email,
                Password = userDto.Password,
                FullName = userDto.FullName,
                PhoneNumber = userDto.PhoneNumber,
                IsCoordinator = userDto.IsCoordinator,
                Experience = userDto.Experience,
                Skills = userDto.Skills
                // Bemærk: CreatedAt sættes typisk ved oprettelsen
            };
        }

        // Opdater eksisterende User model fra UserDTO
        private void UpdateUserFromDTO(User user, UserDTO userDto)
        {
            user.FullName = userDto.FullName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.IsCoordinator = userDto.IsCoordinator;
            user.Experience = userDto.Experience;
            user.Skills = userDto.Skills;
            // Bemærk: Password og CreatedAt opdateres ikke her
        }
    }
}
