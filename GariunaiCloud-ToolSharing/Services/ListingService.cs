using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IList<Listing>> GetListingsAsync()
    {
        return await _context.Listings
            .Include(l => l.Owner )
            .Where(l => l.Hidden == false)
            .ToListAsync();;
    }

    public async Task<IList<Listing>> GetListingsByUserAsync(string userName)
    {
        var user = await _context.Users
            .Include(u => u.Listings.Where(listing => listing.Hidden == false))
            .FirstOrDefaultAsync(u => u.UserName == userName );

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
}