using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;
using static System.String;

namespace GariunaiCloud_ToolSharing.Services;

public class ListingService : IListingService
{
    private readonly GariunaiDbContext _context;
    private readonly IUserService _userService;

    public ListingService(GariunaiDbContext gariunaiDbContext, IUserService userService)
    {
        _context = gariunaiDbContext;
        _userService = userService;
    }

    public async Task<Listing?> GetListingAsync(long listingId)
    {
        var listing = await _context
            .Listings
            .Include(l => l.Owner)
            .Include(l => l.Image)
            .FirstOrDefaultAsync(l => l.ListingId == listingId && l.Hidden == false);
        return listing;
    }

    public async Task<long> CreateListingAsync(Listing listing, string userName)
    {
        var owner = await _userService.GetUserByUsernameAsync(userName);

        if (owner == null)
            throw new KeyNotFoundException();

        listing.Owner = owner;

        await _context.Listings.AddAsync(listing);
        await _context.SaveChangesAsync();
        return listing.ListingId;
    }

    public async Task<IList<Listing>> GetListingsAsync(ListingsFilter filter)
    {
        var listings = await _context.Listings
            .Include(l => l.Owner)
            .Where(l => l.Hidden == false)
            .AsNoTracking()
            .ToListAsync(); //TODO: optimize

        var filteredListings = listings
            .Where(l => _filterListing(l, filter));

        var listingsSorted = filter.SortOrder switch
        {
            ListingSortOrder.NameAsc => filteredListings.OrderBy(l => l.Title.ToLower()),
            ListingSortOrder.NameDesc => filteredListings.OrderByDescending(l => l.Title.ToLower()),
            ListingSortOrder.PriceAsc => filteredListings.OrderBy(l => l.DaysPrice),
            ListingSortOrder.PriceDesc => filteredListings.OrderByDescending(l => l.DaysPrice),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return listingsSorted.ToList();
    }

    private bool _filterListing(Listing listing, ListingsFilter filter)
    {
        if (!IsNullOrEmpty(filter.Username))
        {
            if (listing.Owner.Username != filter.Username)
                return false;
        }

        if (!IsNullOrEmpty(filter.TitleQuery))
        {
            if (listing.Title == null) return false;
            if (!listing.Title.ToLower().Contains(filter.TitleQuery.ToLower())) return false;
        }

        if (!IsNullOrEmpty(filter.City))
        {
            if (listing.City == null) return false;
            if (!string.Equals(listing.City, filter.City, StringComparison.CurrentCultureIgnoreCase)) return false;
        }

        if (filter.MaxPrice != null && listing.DaysPrice > filter.MaxPrice) return false;

        return true;
    }

    public async Task<IList<Listing>> GetListingsByUserAsync(string userName)
    {
        var user = await _context.Users
            .Include(u => u.Listings.Where(listing => listing.Hidden == false))
            .FirstOrDefaultAsync(u => u.Username == userName);

        if (user == null)
            throw new KeyNotFoundException();

        return user.Listings;
    }

    public Task<bool> ListingExistsAsync(long listingId)
    {
        return _context.Listings.AnyAsync(l => l.ListingId == listingId);
    }

    public async Task<bool> IsAvailableToRentAsync(long listingId, DateTime startDate, DateTime endDate)
    {
        if (!ListingExistsAsync(listingId).Result)
            return false;

        var busy = await _context
            .Listings
            .AsNoTracking()
            .Where(l => l.ListingId == listingId)
            .Include(l => l.Orders)
            .SelectMany(l => l.Orders)
            .AnyAsync(o =>
                o.Status != OrderStatus.Pending
                && o.Status != OrderStatus.Cancelled
                && o.EndDate <= endDate
                && o.StartDate >= startDate
            );

        return !busy;
    }

    public async Task<IList<DateTime>> GetUnavailableDatesAsync(long listingId, DateTime startDate, DateTime endDate)
    {
        var orders = await _context
            .Listings
            .AsNoTracking()
            .Where(l => l.ListingId == listingId)
            .Include(l => l.Orders)
            .SelectMany(l => l.Orders)
            .Where(o =>
                o.Status != OrderStatus.Pending
                && o.Status != OrderStatus.Cancelled
                && o.EndDate <= endDate
                && o.StartDate >= startDate)
            .ToListAsync();

        var unavailableDates = new HashSet<DateTime>();
        foreach (var order in orders)
        {
            for (var date = order.StartDate; date <= order.EndDate; date = date.AddDays(1))
            {
                unavailableDates.Add(date.Date);
            }
        }

        return unavailableDates.ToList();
    }

    public async Task<Listing?> UpdateListingInfoAsync(Listing listing)
    {
        var existingListing = await _context.Listings
            .Include(l => l.Owner)
            .FirstOrDefaultAsync(l => l.ListingId == listing.ListingId);

        if (existingListing == null)
        {
            return null;
        }
        
        var dbVersion = existingListing.Version.ToArray();
        
        existingListing.City = listing.City;
        existingListing.Deposit = listing.Deposit;
        existingListing.Description = listing.Description;
        existingListing.Title = listing.Title;
        existingListing.DaysPrice = listing.DaysPrice;

        _context.Entry(existingListing).Property("Version").OriginalValue = listing.Version.ToArray();

        //throws on concurrency conflict
        await _context.SaveChangesAsync();
        return existingListing;
    }

    public async Task<bool> IsByUser(long listingId, string userName)
    {
        var existingListing = await _context.Listings
            .AsNoTracking()
            .Include(u => u.Owner)
            .FirstOrDefaultAsync(l => l.ListingId == listingId);
        return existingListing != null && existingListing.Owner.Username == userName;
    }

    public async Task DeleteListingAsync(long listingId)
    {
        var listing = await _context.Listings
            .FirstOrDefaultAsync(l => l.ListingId == listingId);

        if (listing == null) return;
        _context.Listings.Remove(listing);
        await _context.SaveChangesAsync();
    }

    public async Task<IList<Listing>?> SearchListingsAsync(string searchString)
    {
        if (!IsNullOrEmpty(searchString))
        {
            return await _context.Listings
                .Include(l => l.Owner)
                .Where(l => l.Hidden == false && l.Title.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();
        }

        return null;
    }

    public async Task SetImageAsync(long listingId, DbImage image)
    {
        var listing = _context.Listings.FirstOrDefault(l => l.ListingId == listingId);
        if (listing == null)
            return;

        listing.Image = image;
        await _context.SaveChangesAsync();
    }
}