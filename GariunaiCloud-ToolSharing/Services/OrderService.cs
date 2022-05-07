using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace GariunaiCloud_ToolSharing.Services;

public class OrderService : IOrderService
{
    private readonly GariunaiDbContext _context;
    private readonly IListingService _listingService;

    public OrderService(GariunaiDbContext context, IListingService listingService)
    {
        _context = context;
        _listingService = listingService;
    }

    public Task<List<Order>> GetPlacedOrdersByUserAsync(string userName)
    {
        return _context
            .Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.Listing)
            .Where(o => o.User.UserName == userName)
            .ToListAsync();
    }

    public Task<List<Order>> GetReceivedOrdersByUserAsync(string userName)
    {
        return _context
            .Orders
            .AsNoTracking()
            .Include(o => o.Listing)
            .Include(o => o.Listing.Owner)
            .Include(o => o.User)
            .Where(o => o.Listing.Owner.UserName == userName)
            .ToListAsync();
    }

    public async Task<Order?> PlaceOrderAsync(string userName, long listingId, DateTime startDate, DateTime endDate)
    {
        
        if(startDate > endDate)
        {
            return null;
        }
        
        if (!_listingService.ListingExistsAsync(listingId).Result)
        {
            return null;
        }
        
        if (!_listingService.IsAvailableToRentAsync(listingId, startDate, endDate).Result)
        {
            return null;
        }

        var listing = await _context.Listings
            .FirstOrDefaultAsync(l => l.ListingId == listingId);
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
        var now = DateTime.Now;
        var duration = (endDate - startDate).Days + 1;
        var price = listing!.DaysPrice * duration;

        var order = new Order
        {
            Listing = listing,
            User = user,
            PlacementDateTime = now,
            StartDate = startDate,
            EndDate = endDate,
            Status = OrderStatus.Pending,
            Price = price,
            Deposit = listing.Deposit
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        _context.Entry(order).State = EntityState.Detached;
        return order;

    }

    public Task<Order?> GetOrderAsync(long orderId)
    {
        return _context
            .Orders
            .AsNoTracking()
            .Include(o => o.Listing)
            .Include(o => o.Listing.Owner)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<Order?> UpdateOrderStatusAsync(long orderId, OrderStatus status)
    {
        var order = await _context.Orders
            .Include(o => o.Listing)
            .Include(o => o.Listing.Owner)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
        
        if (order == null)
        {
            return null;
        }
        
        if (order.Status == status)
        {
            throw new InvalidOperationException("Order status is already set to " + status);
        }

        switch (status)
        {
            case OrderStatus.Pending:
                throw new InvalidOperationException("Order cannot be set to pending status");
                //break;
            case OrderStatus.Confirmed:
                if(order.Status != OrderStatus.Pending)
                {
                    throw new InvalidOperationException("Can't confirm order that is not pending");
                }
                order.Status = status;
                order.ConfirmationDateTime = DateTime.Now;
                break;
            case OrderStatus.Cancelled:
                if (order.Status == OrderStatus.Completed)
                {
                    throw new InvalidOperationException("Can't cancel order that is already completed");
                }
                order.Status = status;
                order.CompletionDateTime = DateTime.Now;
                break;
            case OrderStatus.Completed:
                if (order.Status != OrderStatus.InProgress && order.Status != OrderStatus.Confirmed)
                {
                    throw new InvalidOperationException("Can't complete order that is not confirmed or in progress");
                }
                order.Status = status;  
                order.CompletionDateTime = DateTime.Now;
                break;
            case OrderStatus.InProgress:
                if (order.Status != OrderStatus.Confirmed)
                {
                    throw new InvalidOperationException("Can't set order to in progress that is not confirmed");
                }
                order.Status = status;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
        await _context.SaveChangesAsync();
        _context.Entry(order).State = EntityState.Detached;
        return order;
    }

    public Task<bool> OrderExistsAsync(long orderId)
    {
        return _context.Orders.AnyAsync(o => o.OrderId == orderId);
    }

    public Task<bool> DeleteOrderAsync(long orderId)
    {
        var order = _context.Orders.Find(orderId);
        if (order == null)
        {
            return Task.FromResult(false);
        }

        _context.Remove(order);
        return Task.FromResult(_context.SaveChangesAsync().Result > 0);

    }

    public async Task<bool> IsAuthorizedAsync(long orderId, string userName)
    {
        return await IsOrderPoster(orderId, userName) || await IsOrderReceiver(orderId, userName);
    }

    public async Task<bool> IsOrderPoster(long orderId, string userName)
    {
        var order = await GetOrderAsync(orderId);

        if (order == null)
        {
            return false;
        }
        
        var isPoster = order.User.UserName == userName;
        
        return isPoster;
    }

    public async Task<bool> IsOrderReceiver(long orderId, string userName)
    {
        var order = await GetOrderAsync(orderId);

        if (order == null)
        {
            return false;
        }
        
        var isOwner = order.Listing.Owner.UserName == userName;
        return isOwner;
    }
}