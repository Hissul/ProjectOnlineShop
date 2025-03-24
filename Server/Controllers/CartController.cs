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

    public CartController (CartService cartService) {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCartAsync ([FromBody] AddToCartModel request) {

        string userIdFromToken = User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine ($"UserId из токена: {userIdFromToken}");

        if (string.IsNullOrEmpty(request.UserId) || request.ProductId <= 0) {
            return BadRequest ("Invalid user ID or product ID.");
        }

        await _cartService.AddToCartAsync (request.UserId, request.ProductId);
        return Ok (new { message = "Product added to cart successfully." });
    }

    [HttpGet("user_cart/{userId}")]
    public async Task<IActionResult> GetUserCartAsync (string userId) {
        List<ItemInCartModel> items = await _cartService.GetCartItemsByUserIdAsync(userId);
        return Ok(items);
    }


    [HttpPost("remove")]
    public async Task<IActionResult> RemoveProductFromCartAsync ([FromBody] AddToCartModel request) {

        if (string.IsNullOrEmpty (request.UserId) || request.ProductId <= 0) {
            return BadRequest ("Invalid user ID or product ID.");
        }

        await _cartService.RemoveFromCartAsync (request.UserId, request.ProductId);
        return Ok(new { message = "Product removed from cart successfully." });
    }

}
