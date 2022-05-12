using System.ComponentModel.DataAnnotations;

namespace GariunaiCloud_ToolSharing.Models;

#nullable disable 
public class DbImage
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public byte[] ImageData { get; set; }
}