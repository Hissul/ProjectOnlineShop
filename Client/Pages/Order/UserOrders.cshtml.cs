using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Order
{
    public class UserOrdersModel : PageModel
    {
        private readonly OrderService orderService;
        private readonly IHttpContextAccessor httpContextAccessor;


        public UserOrdersModel (OrderService orderService, IHttpContextAccessor httpContextAccessor) {
            this.orderService = orderService;
            this.httpContextAccessor = httpContextAccessor;
        }


        [BindProperty]
        public List<OrderModel> OrderModels { get; set; }


        public async Task<IActionResult> OnGetAsync (){

            string? userId = httpContextAccessor.HttpContext.Session.GetString("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            OrderModels = await orderService.GetUserOrdersAsync (userId);
            return Page();
        }
    }
}
