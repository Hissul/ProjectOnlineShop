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

        public async Task<IActionResult> OnGetAsync(int orderItemId, int orderId, string? returnUrl) {

            bool result = await _orderService.RemoveOrderItemAsync(orderItemId , orderId);

            return RedirectToPage ("/Admin/Orders/Edit", new { orderId, returnUrl });
            //return RedirectToPage (returnUrl);
        }
    }
}
