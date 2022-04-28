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
    public User Owner { get; set; }
}