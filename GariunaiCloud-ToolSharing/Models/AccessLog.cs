namespace GariunaiCloud_ToolSharing.Models;

#nullable disable
public class AccessLog
{
    public long Id { get; set; }
    public string UserName { get; set; }
    
    public string Role { get; set; }
    public string Method { get; set; }
    public DateTime Time { get; set; }
}