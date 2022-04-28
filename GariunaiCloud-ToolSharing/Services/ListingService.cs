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
            .FirstOrDefaultAsync(l => l.ListingId == listingId);

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
            .Include(l => l.Owner)
            .ToListAsync();
    }

    public async Task<IList<Listing>> GetListingsByUserAsync(string userName)
    {
        var user = await _context.Users
            .Include(u => u.Listings)
            .FirstOrDefaultAsync(u => u.UserName == userName);

        if (user == null)
            throw new KeyNotFoundException();

        return user.Listings;
    }
}