using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IListingService
{
    public Task<Listing?> GetListingAsync(long listingId);
    public Task<long> CreateListingAsync(Listing listing, string userName);
    public Task<IList<Listing>> GetListingsAsync();
    public Task<IList<Listing>> GetListingsByUserAsync(string userName);
}