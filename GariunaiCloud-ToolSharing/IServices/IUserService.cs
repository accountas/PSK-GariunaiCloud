using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IUserService
{
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<User?> RegisterUserAsync(string username, string password);
    public Task<User?> GetByCredentials(string username, string password);
    public Task<User?> UpdateUserInfoAsync(string userName, User user);
    public Task<List<User>> GetUsersAsync();
}