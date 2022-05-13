using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IAccessLogger
{
    Task LogAsync(AccessLog log);
}