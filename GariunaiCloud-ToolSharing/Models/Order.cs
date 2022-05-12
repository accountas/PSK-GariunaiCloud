using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GariunaiCloud_ToolSharing.Models;

#nullable disable
public class Order
{
    public long OrderId { get; set; }
    [Column(TypeName = "Date")] public DateTime StartDate { get; set; }
    [Column(TypeName = "Date")] public DateTime EndDate { get; set; }
    public DateTime PlacementDateTime { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public DateTime? CompletionDateTime { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Price { get; set; }
    public decimal Deposit { get; set; }
    public User User { get; set; }
    public Listing Listing { get; set; }
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Pending, Confirmed, Cancelled, Completed, InProgress
}
