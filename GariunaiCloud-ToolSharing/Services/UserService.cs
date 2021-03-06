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
            .FirstOrDefaultAsync(u => u.Username == username);

        return user;
    }

    public async Task<bool> VerifyNewEmailAsync(string email)
    {
        var emailExists = await _context
            .Users
            .AnyAsync(u => u.Email == email);

        return !emailExists;
    }


    public async Task<User?> RegisterUserAsync(string username, string email, string password)
    {
        if (GetUserByUsernameAsync((username)).Result != null || username == "me")
        {
            return null;
        }

        _passwordHashingStrategy.HashPassword(password, out var hashedPassword, out var salt);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = hashedPassword,
            PasswordSalt = salt
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> AuthenticateByUsername(string username, string password)
    {
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return null;
        }

        return !_passwordHashingStrategy.VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? null : user;
    }

    public async Task<User?> AuthenticateByEmail(string email, string password)
    {
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return null;
        }

        return !_passwordHashingStrategy.VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? null : user;
    }

    public async Task<User?> UpdateUserInfoAsync(string userName, User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == userName);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public Task<List<User>> GetUsersAsync()
    {
        return _context.Users.ToListAsync();
    }

    public Task<bool> UserExistsAsync(string username)
    {
        return _context.Users.AnyAsync(u => u.Username == username);
    }
}