using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Cart
{
    public class UserCartModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserCartModel (CartService cartService, IHttpContextAccessor contextAccessor) {
            _cartService = cartService;
            _contextAccessor = contextAccessor;
        }

        [BindProperty]
        public List<ItemInCart>? CartItems { get; set; }

        public async Task<IActionResult> OnGetAsync(){

            string? userId = _contextAccessor.HttpContext.Session.GetString("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            CartItems = await _cartService.GetUserCartAsync(userId);
            return Page();
        }


    }
}
