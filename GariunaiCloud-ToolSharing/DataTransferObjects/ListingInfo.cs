namespace GariunaiCloud_ToolSharing.DataTransferObjects;

#nullable disable
public class ListingInfo
{
    public long ListingId { get; set; }
    public string OwnerUsername { get; set; }
    public string City { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Deposit { get; set; }
    public decimal DaysPrice { get; set; }
    public bool Hidden { get; set; }
    public string ETag { get; set; }
}