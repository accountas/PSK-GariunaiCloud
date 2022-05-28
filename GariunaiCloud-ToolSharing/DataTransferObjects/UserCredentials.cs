using System.ComponentModel.DataAnnotations;

namespace GariunaiCloud_ToolSharing.DataTransferObjects;

#nullable disable
public class UserCredentials
{
    public string Username { get; set; }

    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
