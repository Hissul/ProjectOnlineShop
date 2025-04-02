using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Order
{
    public class CheckoutModel : PageModel
    {
        private readonly OrderService orderService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CheckoutModel (OrderService orderService, IHttpContextAccessor httpContextAccessor) {
            this.orderService = orderService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGet(){
            string? userId = httpContextAccessor.HttpContext.Session.GetString("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            OrderModel? order = await orderService.CreateOrderAsync (userId);
            return RedirectToPage ("/Order/UserOrders");
        }
    }
}
