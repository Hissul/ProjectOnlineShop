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



        [TempData]
        public string? Notification { get; set; }

        [TempData]
        public string? NotificationType { get; set; }



        public async Task<IActionResult> OnGet(string phone, string address) {

            TempData.Remove ("Notification");
            TempData.Remove ("NotificationType");

            string? userId = httpContextAccessor.HttpContext.Session.GetString("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            OrderModel? order = await orderService.CreateOrderAsync (userId, phone, address);

            if (order != null) {
                Notification = "Ваш заказ оформлен.";
                NotificationType = "success";
            }
            else {
                Notification = "Ошибка при оформлении заказа. Повторите попытку позже";
                NotificationType = "error";
            }

            return RedirectToPage ("/Order/UserOrders");
        }
    }
}
