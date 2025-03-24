using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Cart
{
    public class RemoveModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly IHttpContextAccessor _contextAccessor;

        public RemoveModel (CartService cartService, IHttpContextAccessor contextAccessor) {
            _cartService = cartService;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> OnGet(int productId){
            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            await _cartService.RemoveProductFromCartAsync (userId, productId);
            return RedirectToPage ("UserCart");
        }
    }
}
