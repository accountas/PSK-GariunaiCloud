using GariunaiCloud_ToolSharing.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace GariunaiCloud_ToolSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : Controller
{
    private readonly GariunaiDbContext _context;

    public LogsController(GariunaiDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Get access logs. (For debug purposes)
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.AccessLogs.ToList().OrderByDescending(l => l.Time).ToList());
    }
}