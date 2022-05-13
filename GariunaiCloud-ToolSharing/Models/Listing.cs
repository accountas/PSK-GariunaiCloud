using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace GariunaiCloud_ToolSharing.Models;

#nullable disable
public class Listing
{
    public long ListingId { get; set; }
    public string City { get; set; }
    public decimal DaysPrice { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Deposit { get; set; }
    public bool Hidden { get; set; }
    public User Owner { get; set; }
    public List<Order> Orders { get; set; }
    public DbImage Image { get; set; }
    [Timestamp] public byte[] Version { get; set; }
}