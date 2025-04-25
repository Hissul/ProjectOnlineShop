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
    private readonly ILogger<OrderController> _logger;


    public OrderController (OrderService orderService, ILogger<OrderController> logger) {
        this.orderService = orderService;
        _logger = logger;
    }


    [HttpGet ("create/{userId}/{phone}/{address}")]
    public async Task<IActionResult> CreateOrderAsync (string userId, string phone, string address) {
        _logger.LogInformation ("Создание заказа для пользователя {UserId}", userId);
        OrderModel order = await orderService.CreateOrderAsync (userId, phone, address);
        return Ok (order);
    }

    [HttpGet ("all/{userId}")]
    public async Task<IActionResult> GetUserOrdersAsync (string userId) {
        _logger.LogInformation ("Получение заказов пользователя {UserId}", userId);
        var orders = await orderService.GetUserOrdersAsync (userId);
        return Ok (orders);
    }

    [HttpGet ("info/{orderId}")]
    public async Task<IActionResult> GetOrderFullInfoAsync (int orderId) {
        _logger.LogInformation ("Получение полной информации о заказе {OrderId}", orderId);
        var order = await orderService.GetOrderFullInfoAsync (orderId);
        return Ok (order);
    }

    [HttpGet ("all_by_status/{status}")]
    public async Task<IActionResult> GetOrdersByStatusAsync (string status) {
        _logger.LogInformation ("Получение заказов по статусу: {Status}", status);
        var orders = await orderService.GetOrdersByStatusAsync (status);
        return Ok (orders);
    }

    [HttpPost ("edit/{orderId}/{orderStatus}")]
    public async Task<IActionResult> EditOrderAsync (int orderId, string orderStatus) {
        _logger.LogInformation ("Изменение статуса заказа {OrderId} на {Status}", orderId, orderStatus);
        await orderService.EditOrderAsync (orderId, orderStatus);
        return Ok ();
    }

    [HttpDelete ("remove_order_item/{orderItemId}/{orderId}")]
    public async Task<IActionResult> RemoveOrderItemAsync (int orderItemId, int orderId) {
        _logger.LogInformation ("Удаление позиции {OrderItemId} из заказа {OrderId}", orderItemId, orderId);
        await orderService.RemoveOrderItemAsync (orderItemId, orderId);
        return Ok ();
    }

}
