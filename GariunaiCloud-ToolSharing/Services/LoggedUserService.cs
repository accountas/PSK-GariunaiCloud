using System.Diagnostics;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.Services;

public class LoggedUserService : IUserService
{
    private readonly IUserService _userService;
    private readonly IAccessLogger _accessLogger;
    private readonly string _username;
    private readonly string _role;

    public LoggedUserService(IUserService userService, IAccessLogger accessLogger,
        IHttpContextAccessor httpContextAccessor)
    {
        Debug.Assert(httpContextAccessor.HttpContext != null, "httpContextAccessor.HttpContext != null");
        _userService = userService;
        _accessLogger = accessLogger;
        _username = httpContextAccessor.HttpContext.User.GetUsername();
        _role = httpContextAccessor.HttpContext.User.GetRole();
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _userService.GetUserByUsernameAsync(username);
    }

    public Task<bool> VerifyNewEmailAsync(string email)
    {
        return _userService.VerifyNewEmailAsync(email);
    }

    public Task<User?> RegisterUserAsync(string username, string email, string password)
    {
        return _userService.RegisterUserAsync(username, email, password);
    }

    public async Task<User?> AuthenticateByUsername(string username, string password)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(UpdateUserInfoAsync), username));
        return await _userService.AuthenticateByUsername(username, password);
    }

    public async Task<User?> AuthenticateByEmail(string email, string password)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(UpdateUserInfoAsync), email));
        return await _userService.AuthenticateByEmail(email, password);
    }

    public async Task<User?> UpdateUserInfoAsync(string userName, User user)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(UpdateUserInfoAsync)));
        return await _userService.UpdateUserInfoAsync(userName, user);
    }

    public Task<List<User>> GetUsersAsync()
    {
        return _userService.GetUsersAsync();
    }

    public Task<bool> UserExistsAsync(string username)
    {
        return _userService.UserExistsAsync(username);
    }

    private AccessLog _getMethodAccessLog(string method, string? username = null)
    {
        return new AccessLog
        {
            UserName = username ?? _username,
            Method = _userService.GetType().Name + "." + method,
            Role = _role,
            Time = DateTime.Now
        };
    }
}