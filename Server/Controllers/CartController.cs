using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;
using System.Security.Claims;

namespace Server.Controllers;

[ApiController]
[Route("cart")]
[Authorize]
public class CartController : Controller {
   
    private readonly CartService _cartService;
    private readonly ILogger<CartController> _logger;

    public CartController (CartService cartService, ILogger<CartController> logger) {
        _cartService = cartService;
        _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCartAsync ([FromBody] AddToCartModel request) {

        string userIdFromToken = User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation ("Запрос на добавление товара в корзину. Пользователь из токена: {UserIdToken}, UserId из тела: {UserIdBody}", userIdFromToken, request.UserId);

        if (string.IsNullOrEmpty (request.UserId) || request.ProductId <= 0) {
            _logger.LogWarning ("Невалидный запрос: отсутствует userId или productId");
            return BadRequest ("Invalid user ID or product ID.");
        }

        await _cartService.AddToCartAsync (request.UserId, request.ProductId);
        return Ok (new { message = "Product added to cart successfully." });
    }

    [HttpGet("user_cart/{userId}")]
    public async Task<IActionResult> GetUserCartAsync (string userId) {
        _logger.LogInformation ("Получение корзины пользователя {UserId}", userId);

        var items = await _cartService.GetCartItemsByUserIdAsync (userId);
        return Ok (items);
    }


    [HttpPost("remove")]
    public async Task<IActionResult> RemoveProductFromCartAsync ([FromBody] AddToCartModel request) {

        _logger.LogInformation ("Запрос на удаление товара {ProductId} из корзины пользователя {UserId}", request.ProductId, request.UserId);

        if (string.IsNullOrEmpty (request.UserId) || request.ProductId <= 0) {
            _logger.LogWarning ("Невалидный запрос: отсутствует userId или productId");
            return BadRequest ("Invalid user ID or product ID.");
        }

        await _cartService.RemoveFromCartAsync (request.UserId, request.ProductId);
        return Ok (new { message = "Product removed from cart successfully." });
    }

}
