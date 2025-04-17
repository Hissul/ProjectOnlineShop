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


        [TempData]
        public string? Notification { get; set; }

        [TempData]
        public string? NotificationType { get; set; }



        public async Task<IActionResult> OnGet(int productId){

            TempData.Remove ("Notification");
            TempData.Remove ("NotificationType");

            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            bool result = await _cartService.RemoveProductFromCartAsync (userId, productId);

            if (result) {
                Notification = "Товар удален из корзины.";
                NotificationType = "success";
            }
            else {
                Notification = "Ошибка при удалении товара из корзины. Повторите попытку позже";
                NotificationType = "error";
            }
            return RedirectToPage ("UserCart");
        }
    }
}
