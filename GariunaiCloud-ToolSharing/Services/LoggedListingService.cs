using System.Diagnostics;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.Services;

public class LoggedListingService : IListingService
{
    private readonly IListingService _listingService;
    private readonly IAccessLogger _accessLogger;
    private readonly string _username;
    private readonly string _role;

    public LoggedListingService(IListingService listingService, IHttpContextAccessor httpContextAccessor,
        IAccessLogger accessLogger)
    {
        Debug.Assert(httpContextAccessor.HttpContext != null, "httpContextAccessor.HttpContext != null");
        _listingService = listingService;
        _accessLogger = accessLogger;
        _username = httpContextAccessor.HttpContext.User.GetUsername();
        _role = httpContextAccessor.HttpContext.User.GetRole();
    }

    public async Task<Listing?> GetListingAsync(long listingId)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(GetListingAsync)));
        return await _listingService.GetListingAsync(listingId);
    }

    public async Task<long> CreateListingAsync(Listing listing, string userName)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(CreateListingAsync)));
        return await _listingService.CreateListingAsync(listing, userName);
    }

    public async Task<IList<Listing>> GetListingsAsync(ListingsFilter filters)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(GetListingsAsync)));
        return await _listingService.GetListingsAsync(filters);
    }

    public async Task<IList<Listing>> GetListingsByUserAsync(string userName)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(GetListingsByUserAsync)));
        return await _listingService.GetListingsByUserAsync(userName);
    }

    public async Task<bool> ListingExistsAsync(long listingId)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(ListingExistsAsync)));
        return await _listingService.ListingExistsAsync(listingId);
    }

    public async Task<bool> IsAvailableToRentAsync(long listingId, DateTime startDate, DateTime endDate)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(IsAvailableToRentAsync)));
        return await _listingService.IsAvailableToRentAsync(listingId, startDate, endDate);
    }

    public async Task<IList<DateTime>> GetUnavailableDatesAsync(long listingId, DateTime startDate, DateTime endDate)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(GetUnavailableDatesAsync)));
        return await _listingService.GetUnavailableDatesAsync(listingId, startDate, endDate);
    }

    public async Task<Listing?> UpdateListingInfoAsync(Listing listing, bool force)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(UpdateListingInfoAsync)));
        return await _listingService.UpdateListingInfoAsync(listing, force);
    }

    public async Task DeleteListingAsync(long listingId)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(DeleteListingAsync)));
        await _listingService.DeleteListingAsync(listingId);
    }

    public async Task<bool> IsByUser(long listingId, string userName)
    {
        return await _listingService.IsByUser(listingId, userName);
    }

    public async Task SetImageAsync(long listingId, DbImage image)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(SetImageAsync)));
        await _listingService.SetImageAsync(listingId, image);
    }

    private AccessLog _getMethodAccessLog(string method)
    {
        return new AccessLog
        {
            UserName = _username,
            Method = _listingService.GetType().Name + "." + method,
            Role = _role,
            Time = DateTime.Now
        };
    }
}