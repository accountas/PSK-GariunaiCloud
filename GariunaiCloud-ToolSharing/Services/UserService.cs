using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace GariunaiCloud_ToolSharing.Services;

public class UserService : IUserService
{
    private readonly GariunaiDbContext _context;

    public UserService(GariunaiDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        var user = _context
            .Users
            .FirstOrDefaultAsync(u => u.UserName == username);

        return user;
    }

    public  Task<List<User>> GetUsersAsync()
    {
        return _context.Users.ToListAsync();
    }
}