using System.Security.Claims;

namespace GariunaiCloud_ToolSharing;

public static class Extensions
{
    public static string GetUsername(this ClaimsPrincipal principal)
    {
        return principal.Claims.First(c => c.Type == ClaimTypes.Name).Value;
    }
    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
    }

}