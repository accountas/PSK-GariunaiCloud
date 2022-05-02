using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IJwtTokenService
{
    public string GenerateJwtToken(User user);
}