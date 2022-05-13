using System.Text.Json.Serialization;

namespace GariunaiCloud_ToolSharing.Models;

public class ListingsFilter
{
    public string? TitleQuery { get; set; }
    public string? City { get; set; }
    public decimal? MaxPrice { get; set; }
    public ListingSortOrder SortOrder { get; set; }
    public string? Username { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ListingSortOrder
{
    NameAsc, NameDesc, PriceAsc, PriceDesc
}