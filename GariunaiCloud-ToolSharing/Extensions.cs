using System.Security.Claims;

namespace GariunaiCloud_ToolSharing;

public static class Extensions
{
    public static string GetUsername(this ClaimsPrincipal principal)
    {
        var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        return claim == null ? "Anonymous" : claim.Value;
    }
    
    public static string GetRole(this ClaimsPrincipal principal)
    {
        var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        return claim == null ? "Anonymous" : claim.Value;
    }
}