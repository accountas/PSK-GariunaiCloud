﻿using System.ComponentModel.DataAnnotations;

namespace GariunaiCloud_ToolSharing.PresentationLayer.DataTransferObjects;

#nullable disable
public class NewListingPayload
{
    [Required] public string City { get; set; }

    [Required] public string Title { get; set; }

    [Required] public string Description { get; set; }

    [Required] public decimal Deposit { get; set; }

    [Required] public decimal DaysPrice { get; set; }
}