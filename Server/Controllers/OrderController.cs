using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[ApiController]
[Route ("order")]
public class OrderController : Controller {

    private readonly OrderService orderService;

    public OrderController (OrderService orderService) {
        this.orderService = orderService;
    }


    [HttpGet("create/{userId}")]
    public async Task<IActionResult> CreateOrderAsync (string userId) {
        OrderModel order = await orderService.CreateOrderAsync (userId);

        return Ok(order);
    }


    [HttpGet("all/{userId}")]
    public async Task<IActionResult> GetUserOdersAync (string userId) {
        List<OrderModel> userOrders = await orderService.GetUserOrdersAsync (userId);

        return Ok(userOrders);
    }


    [HttpGet("info/{orderId}")]
    public async Task<IActionResult> GetOrderFullInfoAsync(int orderId) {
        OrderModel? order = await orderService.GetOrderFullInfoAsync (orderId);
        return Ok(order);
    }

}
