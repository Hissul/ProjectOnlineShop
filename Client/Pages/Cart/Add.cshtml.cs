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


        [TempData]
        public string? Notification { get; set; }

        [TempData]
        public string? NotificationType { get; set; }     


        public async Task<IActionResult> OnGetAsync(int productId) {

            TempData.Remove ("Notification");
            TempData.Remove ("NotificationType");

            string? userId = _contextAccessor.HttpContext.Session.GetString("user_id");

            if(userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            bool result = await _cartService.AddToCartAsync (userId, productId);

            if (result) {
                Notification = "Товар добавлен в корзину.";
                NotificationType = "success";
            }
            else {
                Notification = "Ошибка при добавлении товара корзину. Повторите попытку позже";
                NotificationType = "error";
            }            

            return  RedirectToPage("UserCart");
            
        }
    }
}
