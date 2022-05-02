using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace GariunaiCloud_ToolSharing.Services;

public class UserService : IUserService
{
    private readonly GariunaiDbContext _context;
    private readonly IPasswordHashingStrategy _passwordHashingStrategy;

    public UserService(GariunaiDbContext context, IPasswordHashingStrategy passwordHashingStrategy)
    {
        _context = context;
        _passwordHashingStrategy = passwordHashingStrategy;
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        var user = _context
            .Users
            .FirstOrDefaultAsync(u => u.UserName == username);

        return user;
    }

    public async Task<User?> RegisterUserAsync(string username, string password)
    {
        if(GetUserByUsernameAsync((username)).Result != null)
        {
            return null;
        }
        
        _passwordHashingStrategy.HashPassword(password, out var hashedPassword, out var salt);
        
        var user = new User
        {
            UserName = username,
            PasswordHash = hashedPassword,
            PasswordSalt = salt
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByCredentials(string username, string password)
    {
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.UserName == username);
        
        if(user == null)
        {
            return null;
        }
        
        return !_passwordHashingStrategy.VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? null : user;
    }

    public async Task<User?> UpdateUserInfoAsync(string userName, User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        
        await _context.SaveChangesAsync();
        return existingUser;
    }
    
    public  Task<List<User>> GetUsersAsync()
    {
        return _context.Users.ToListAsync();
    }
}