using System.ComponentModel.DataAnnotations;

namespace GariunaiCloud_ToolSharing.DataTransferObjects;

public class NewOrderPayload
{
    [Required] public long ListingId { get; set; }

    [Required] public DateTime StartDate { get; set; }

    [Required] public DateTime EndDate { get; set; }
}