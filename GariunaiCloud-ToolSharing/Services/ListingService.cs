using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using GariunaiCloud_ToolSharing.PresentationLayer.DataTransferObjects;
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

    public async Task<Listing?> UpdateListingInfoAsync(Listing listing)
    {
        var existingListing  = await _context.Listings
            .Include(l => l.Owner)
            .FirstOrDefaultAsync(l => l.ListingId == listing.ListingId);

        if (existingListing == null)
        {
            return null;
        }
        
        existingListing.City = listing.City;
        existingListing.Deposit = listing.Deposit;
        existingListing.Description = listing.Description;
        existingListing.Title = listing.Title;
        existingListing.DaysPrice = listing.DaysPrice;

        await _context.SaveChangesAsync();
        return listing;
    }

    public async Task<bool> IsByUser(long listingId, string userName)
    {
        var existingListing  = await _context.Listings
            .AsNoTracking()
            .Include(u=> u.Owner)
            .FirstOrDefaultAsync(l => l.ListingId == listingId);
        return existingListing != null && existingListing.Owner.UserName == userName;
    }
    public async Task DeleteListingAsync(long listingId)
    {
        var listing  = await _context.Listings
            .FirstOrDefaultAsync(l => l.ListingId == listingId);
        _context.Listings.Remove(listing);
        await _context.SaveChangesAsync();
    }
}