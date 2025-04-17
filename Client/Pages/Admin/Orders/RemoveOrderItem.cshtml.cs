using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Admin.Orders
{
    public class RemoveOrderItemModel : PageModel
    {
        private readonly OrderService _orderService;

        public RemoveOrderItemModel (OrderService orderService) {
            _orderService = orderService;
        }


        [TempData]
        public string? Notification { get; set; }

        [TempData]
        public string? NotificationType { get; set; }


        public async Task<IActionResult> OnGetAsync(int orderItemId, int orderId, string? returnUrl) {

            TempData.Remove ("Notification");
            TempData.Remove ("NotificationType");

            bool result = await _orderService.RemoveOrderItemAsync(orderItemId , orderId);

            if (result) {
                Notification = "Товар удален из заказа.";
                NotificationType = "success";                
            }
            else {
                Notification = "Ошибка удалении товара.";
                NotificationType = "error";
            }

            return RedirectToPage ("/Admin/Orders/Edit", new { orderId, returnUrl });
            //return RedirectToPage (returnUrl);
        }
    }
}
