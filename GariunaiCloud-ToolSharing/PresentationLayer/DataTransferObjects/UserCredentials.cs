using System.ComponentModel.DataAnnotations;

namespace GariunaiCloud_ToolSharing.PresentationLayer.DataTransferObjects;

#nullable disable
public class UserCredentials
{
    [Required]
    public string Username { get; set; }

    [Required] 
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}