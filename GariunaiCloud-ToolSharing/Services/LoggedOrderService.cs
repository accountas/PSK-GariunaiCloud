using System.Diagnostics;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.Services;

public class LoggedOrderService : IOrderService
{
    private readonly IOrderService _orderService;
    private readonly IAccessLogger _accessLogger;
    private readonly string _username;
    private readonly string _role;

    public LoggedOrderService(IOrderService orderService, IAccessLogger accessLogger,
        IHttpContextAccessor httpContextAccessor)
    {
        Debug.Assert(httpContextAccessor.HttpContext != null, "httpContextAccessor.HttpContext != null");
        _orderService = orderService;
        _accessLogger = accessLogger;
        _username = httpContextAccessor.HttpContext.User.GetUsername();
        _role = httpContextAccessor.HttpContext.User.GetRole();
    }

    public async Task<List<Order>> GetPlacedOrdersByUserAsync(string userName)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(GetPlacedOrdersByUserAsync)));
        return await _orderService.GetPlacedOrdersByUserAsync(userName);
    }

    public async Task<List<Order>> GetReceivedOrdersByUserAsync(string userName)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(GetReceivedOrdersByUserAsync)));
        return await _orderService.GetReceivedOrdersByUserAsync(userName);
    }

    public async Task<Order?> PlaceOrderAsync(string userName, long listingId, DateTime startDate, DateTime endDate)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(PlaceOrderAsync)));
        return await _orderService.PlaceOrderAsync(userName, listingId, startDate, endDate);
    }

    public async Task<Order?> GetOrderAsync(long orderId)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(GetOrderAsync)));
        return await _orderService.GetOrderAsync(orderId);
    }

    public async Task<Order?> UpdateOrderStatusAsync(long orderId, OrderStatus status)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(UpdateOrderStatusAsync)));
        return await _orderService.UpdateOrderStatusAsync(orderId, status);
    }

    public async Task<bool> OrderExistsAsync(long orderId)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(OrderExistsAsync)));
        return await _orderService.OrderExistsAsync(orderId);
    }

    public async Task<bool> DeleteOrderAsync(long orderId)
    {
        await _accessLogger.LogAsync(_getMethodAccessLog(nameof(DeleteOrderAsync)));
        return await _orderService.DeleteOrderAsync(orderId);
    }

    public Task<bool> IsAuthorizedAsync(long orderId, string userName)
    {
        return _orderService.IsAuthorizedAsync(orderId, userName);
    }

    public Task<bool> IsOrderPoster(long orderId, string userName)
    {
        return _orderService.IsOrderPoster(orderId, userName);
    }

    public Task<bool> IsOrderReceiver(long orderId, string userName)
    {
        return _orderService.IsOrderReceiver(orderId, userName);
    }

    private AccessLog _getMethodAccessLog(string method)
    {
        return new AccessLog
        {
            UserName = _username,
            Method = _orderService.GetType().Name + "." + method,
            Role = _role,
            Time = DateTime.Now
        };
    }
}