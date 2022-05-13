namespace GariunaiCloud_ToolSharing.Models;

#nullable disable
public class User
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public List<Listing> Listings { get; set; }
    
    public List<Order> Orders { get; set; }
}

public enum UserRole
{
    User,
    Admin
}