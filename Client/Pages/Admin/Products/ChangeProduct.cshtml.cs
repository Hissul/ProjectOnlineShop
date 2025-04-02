using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;
using System.Runtime.CompilerServices;

namespace Client.Pages.Admin.Products
{
    public class ChangeProductModel : PageModel
    {
        private readonly StoreService _storeService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ChangeProductModel (StoreService storeService, IHttpContextAccessor contextAccessor) {
            _storeService = storeService;
            _contextAccessor = contextAccessor;
        }

        [BindProperty]
        public List<ProductFullModel> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(){
            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            Products = await _storeService.GetAllProductFullAsync ();
            return Page();
        }
    }
}
