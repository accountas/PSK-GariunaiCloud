using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.DataTransferObjects;

#nullable disable
public class OrderPayload
{
    public long OrderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PlacementDateTime { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public DateTime? CompletionDateTime { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Price { get; set; }
    public decimal Deposit { get; set; }
    public string PlacerUsername { get; set; }
    public long ListingId { get; set; }
}