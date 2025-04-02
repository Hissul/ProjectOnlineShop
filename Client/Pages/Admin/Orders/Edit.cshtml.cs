using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditModel (OrderService orderService, IHttpContextAccessor contextAccessor) {
            _orderService = orderService;
            _contextAccessor = contextAccessor;
        }

        [BindProperty]
        public OrderModel? Order { get; set; }
        [BindProperty]
        public string? ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId, string? returnUrl)
        {
            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            Order = await _orderService.GetOrderFullInfoAsync (orderId);
            // Сохраняем URL возврата
            ReturnUrl = returnUrl ?? "/Admin/Statistics/Statistic";

            return Page();
        }


        public async Task<IActionResult> OnPostAsync () {

            bool result = await _orderService.EditOrderAsync (Order.Id, Order.Status);

            if(result)
                return RedirectToPage (ReturnUrl);

            return Page();
        }


    }
}
