using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IOrderService
{
    public Task<List<Order>> GetPlacedOrdersByUserAsync(string userName);
    public Task<List<Order>> GetReceivedOrdersByUserAsync(string userName);
    public Task<Order?> PlaceOrderAsync(string userName, long listingId, DateTime startDate, DateTime endDate);
    public Task<Order?> GetOrderAsync(long orderId);
    public Task<Order?> UpdateOrderStatusAsync(long orderId, OrderStatus status);
    public Task<bool> OrderExistsAsync(long orderId);
    public Task<bool> DeleteOrderAsync(long orderId);
    public Task<bool> IsAuthorizedAsync(long orderId, string userName);
    public Task<bool> IsOrderPoster(long orderId, string userName);
    public Task<bool> IsOrderReceiver(long orderId, string userName);
}