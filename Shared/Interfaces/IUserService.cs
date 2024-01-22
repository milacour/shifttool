using ShiftTool.Shared.DTOs;


namespace ShiftTool.Shared.Interfaces
{
    public interface IUserService
    {
        Task<HttpResponseMessage> RegisterUser(UserDTO userDto);
        Task<HttpResponseMessage> LoginAsync(UserDTO userDto);
        Task<HttpResponseMessage> LogoutAsync();
        Task<HttpResponseMessage> CreateUserAsync(UserDTO userDto);
        Task<List<UserDTO>> GetUsers();
        Task<UserDTO> GetCurrentUserAsync(string email);
        Task<HttpResponseMessage> UpdateUserAsync(string email, UserDTO userDto);
        Task<HttpResponseMessage> DeleteUser(string email);
        Task<bool> HandleLogin(UserDTO userDto, bool navigateToProfile);
    }
}
