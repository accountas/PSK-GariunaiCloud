using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.Services;

public class AccessLogger : IAccessLogger
{
    private readonly GariunaiDbContext _context;

    public AccessLogger(GariunaiDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(AccessLog log)
    {
        await _context.AccessLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}