﻿namespace GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;

#nullable disable
public class ListingPayload
{
    public long ListingId { get; set; }
    public string OwnerUsername { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Deposit { get; set; }
    public decimal DaysPrice { get; set; }
}