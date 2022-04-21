using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.Services;

public interface IUserService
{
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<List<User>> GetUsersAsync();
}