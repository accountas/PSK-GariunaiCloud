using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IUserService
{
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<bool> VerifyNewEmailAsync(string email);
    public Task<User?> RegisterUserAsync(string username, string email, string password);
    public Task<User?> AuthenticateByUsername(string username, string password);
    public Task<User?> AuthenticateByEmail(string email, string password);
    public Task<User?> UpdateUserInfoAsync(string userName, User user);
    public Task<List<User>> GetUsersAsync();
}