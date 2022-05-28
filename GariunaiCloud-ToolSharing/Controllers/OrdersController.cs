using AutoMapper;
using GariunaiCloud_ToolSharing.DataTransferObjects;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GariunaiCloud_ToolSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : Controller
{
    private readonly IUserService _userService;
    private readonly IListingService _listingService;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    
    public OrdersController(IUserService userService, IOrderService orderService, IListingService listingService, IMapper mapper)
    {
        _userService = userService;
        _orderService = orderService;
        _listingService = listingService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="newOrder">New order</param>
    /// <returns>The created order</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder(NewOrderPayload newOrder)
    {
        var userName = User.GetUsername();
        
        if(!_listingService.ListingExistsAsync(newOrder.ListingId).Result)
        {
            return NotFound("Listing does not exist");
        }
        if(newOrder.StartDate > newOrder.EndDate)
        {
            return BadRequest("Start date must be before end date");
        }
        if(!_listingService.IsAvailableToRentAsync(newOrder.ListingId, newOrder.StartDate, newOrder.EndDate).Result)
        {
            return BadRequest("Listing is not available to rent in this time period");
        }
        
        var order = await _orderService.PlaceOrderAsync(
            userName,
            newOrder.ListingId,
            newOrder.StartDate,
            newOrder.EndDate);
    
        var dto = _mapper.Map<OrderPayload>(order);
        return Ok(dto);
    }
    
    /// <summary>
    /// Get all orders that you have placed
    /// </summary>
    /// <returns>A list of orders</returns>
    [HttpGet("placed")]
    [Authorize]
    public async Task<IActionResult> GetPlacedOrders()
    {   
        var userName = User.GetUsername();
        var orders = await _orderService.GetPlacedOrdersByUserAsync(userName);
        var dtos = _mapper.Map<List<OrderPayload>>(orders);
        return Ok(dtos);
    }
    
    /// <summary>
    /// Get a list of orders that you have received
    /// </summary>
    /// <returns>A list of orders</returns>
    [HttpGet("received")]
    [Authorize]
    public async Task<IActionResult> GetReceivedOrders()
    {   
        var userName = User.GetUsername();
        var orders = await _orderService.GetReceivedOrdersByUserAsync(userName);
        var dtos = _mapper.Map<List<OrderPayload>>(orders);
        return Ok(dtos);
    }
    
    /// <summary>
    /// Get order by id
    /// </summary>
    /// <param name="id">Order id</param>
    /// <returns>The order</returns>
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetOrder(long id)
    {   
        var userName = User.GetUsername();
        if (!_orderService.IsAuthorizedAsync(id, userName).Result)
        {
            return Unauthorized();
        }
        var order = await _orderService.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        var dto = _mapper.Map<OrderPayload>(order);
        return Ok(dto);
    }

    /// <summary>
    /// Change order status
    /// </summary>
    /// <param name="id">order id</param>
    /// <param name="status">new order status</param>
    /// <param name="force">Ignore status order validation for testing only</param>
    [HttpPost("{id:long}/status")]
    [Authorize]
    public async Task<IActionResult> UpdateOrderStatus(long id, OrderStatus status, [FromQuery] bool force)
    {
        var userName = User.GetUsername();
        if(!_orderService.OrderExistsAsync(id).Result)
        {
            return NotFound();
        }
        if (!_orderService.IsAuthorizedAsync(id, userName).Result)
        {
            return Unauthorized();
        }
        if(!_orderService.IsOrderReceiver(id, userName).Result && status == OrderStatus.Confirmed)
        {
            return Unauthorized();
        }
        
        try
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, status, force);
            var dto = _mapper.Map<OrderPayload>(order);
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
 
    }
    
    /// <summary>
    /// Delete order (only if you are the creator)
    /// </summary>
    /// <param name="id">Order id</param>
    [HttpDelete("{id:long}")]
    [Authorize]
    public async Task<IActionResult> DeleteOrder(long id)
    {
        var userName = User.GetUsername();
        if(!_orderService.OrderExistsAsync(id).Result)
        {
            return NotFound();
        }
        if (!_orderService.IsOrderPoster(id, userName).Result)
        {
            return Unauthorized();
        }
        await _orderService.DeleteOrderAsync(id);
        return Ok();
    }

}