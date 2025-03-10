using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[ApiController]
[Route("cart")]
public class CartController : Controller {
   
    private readonly CartService _cartService;

    public CartController (CartService cartService) {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCartAsync ([FromBody] AddToCartRequest request) {

        if(string.IsNullOrEmpty(request.UserId) || request.ProductId <= 0) {
            return BadRequest ("Invalid user ID or product ID.");
        }

        await _cartService.AddToCartAsync (request.UserId, request.ProductId);
        return Ok (new { message = "Product added to cart successfully." });
    }

    [HttpGet("user_cart/{userId}")]
    public async Task<IActionResult> GetUserCartAsync (string userId) {
        List<ItemInCart> items = await _cartService.GetCartItemsByUserIdAsync(userId);
        return Ok (items);
    }


}
