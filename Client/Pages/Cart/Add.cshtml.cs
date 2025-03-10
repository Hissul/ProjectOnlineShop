using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Cart
{
    public class AddModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddModel (CartService cartService, IHttpContextAccessor contextAccessor) {
            _cartService = cartService;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> OnGetAsync(int productId){
            string? userId = _contextAccessor.HttpContext.Session.GetString("user_id");

            if(userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            await _cartService.AddToCartAsync (userId, productId);
            return RedirectToPage ("UserCart");
        }
    }
}
