using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Admin.Orders
{
    public class CancelledModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CancelledModel (OrderService orderService, IHttpContextAccessor contextAccessor) {
            _orderService = orderService;
            _contextAccessor = contextAccessor;
        }

        [BindProperty]
        public List<OrderModel>? OrderModels { get; set; }

        public async Task<IActionResult> OnGetAsync () {
            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            OrderModels = await _orderService.GetOrdersByStatusAsync ("Cancelled");
            return Page ();
        }

    }
}
