using ShiftTool.Shared.Models;

namespace ShiftTool.Shared.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetCurrentUserAsync(string email);
        Task CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task DeleteUserAsync(string email);
    }
}
