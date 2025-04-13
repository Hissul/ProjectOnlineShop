using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[ApiController]
[Route ("order")]
[Authorize]
public class OrderController : Controller {

    private readonly OrderService orderService;

    public OrderController (OrderService orderService) {
        this.orderService = orderService;
    }


    [HttpGet ("create/{userId}/{phone}/{address}")]
    public async Task<IActionResult> CreateOrderAsync (string userId, string phone, string address) {
        OrderModel order = await orderService.CreateOrderAsync (userId, phone, address);

        return Ok (order);
    }


    [HttpGet ("all/{userId}")]
    public async Task<IActionResult> GetUserOrdersAsync (string userId) {
        List<OrderModel> userOrders = await orderService.GetUserOrdersAsync (userId);

        return Ok (userOrders);
    }


    [HttpGet ("info/{orderId}")]
    public async Task<IActionResult> GetOrderFullInfoAsync (int orderId) {
        OrderModel? order = await orderService.GetOrderFullInfoAsync (orderId);
        return Ok (order);
    }

    [HttpGet("all_by_status/{status}")]
    public async Task<IActionResult> GetOrdersByStatusAsync(string status) {
        List<OrderModel> orders = await orderService.GetOrdersByStatusAsync (status);
        return Ok (orders);
    }



    [HttpPost ("edit/{orderId}/{orderStatus}")]
    public async Task<IActionResult> EditOrderAsync (int orderId, string orderStatus) {
        await orderService.EditOrderAsync (orderId, orderStatus);
        return Ok ();
    }

    [HttpDelete("remove_order_item/{orderItemId}/{orderId}")]
    public async Task<IActionResult> RemoveOrderItemAsync (int orderItemId, int orderId) {
        await orderService.RemoveOrderItemAsync (orderItemId, orderId);
        return Ok ();
    }

}
