using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Order
{
    public class DetailsModel : PageModel
    {
        private readonly OrderService orderService;
        private readonly IHttpContextAccessor httpContextAccessor;


        public DetailsModel (OrderService orderService, IHttpContextAccessor httpContextAccessor) {
            this.orderService = orderService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public OrderModel OrderModel { get; set; }

        public async Task<IActionResult> OnGet(int orderId){

            string? userId = httpContextAccessor.HttpContext.Session.GetString("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            OrderModel = await orderService.GetOrderFullInfoAsync (orderId);

            return Page();
        }
    }
}
