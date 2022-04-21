namespace GariunaiCloud_ToolSharing.Models;

#nullable disable
public class User
{
    public long UserId { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<Listing> Listings { get; set; }
}