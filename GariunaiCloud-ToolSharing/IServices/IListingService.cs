using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IListingService
{
    public Task<Listing?> GetListingAsync(long listingId);
    public Task<long> CreateListingAsync(Listing listing, string userName);
    public Task<IList<Listing>> GetListingsAsync();
    public Task<IList<Listing>> GetListingsByUserAsync(string userName);
    public Task<bool> ListingExistsAsync(long listingId);
    public Task<bool> IsAvailableToRentAsync(long listingId, DateTime startDate, DateTime endDate);
    public Task<IList<DateTime>> GetUnavailableDatesAsync(long listingId, DateTime startDate, DateTime endDate);
    public Task<Listing?> UpdateListingInfoAsync(Listing listing);
    public Task DeleteListingAsync(long listingId);
    public Task<bool> IsByUser(long listingId, string userName);
    public Task<IList<Listing>?> SearchListingsAsync(string searchString);
}