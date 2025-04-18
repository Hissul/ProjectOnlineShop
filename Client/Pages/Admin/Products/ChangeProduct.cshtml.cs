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

        //[BindProperty]
        //public List<ProductFullModel> Products { get; set; }

        //public async Task<IActionResult> OnGetAsync(){
        //    string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

        //    if (userId == null) {
        //        return RedirectToPage ("/Auth/Login");
        //    }

        //    Products = await _storeService.GetAllProductFullAsync ();
        //    return Page();
        //}

        [BindProperty]
        public List<ProductFullModel> Products { get; set; }

        [BindProperty (SupportsGet = true)]
        public int Page { get; set; } = 1;

        [BindProperty (SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync (int pageNumber = 1) {
            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");
            if (userId == null) return RedirectToPage ("/Auth/Login");

            int pageSize = 6;
            var (products, totalCount) = await _storeService.GetPagedProductFullAsync (pageNumber, pageSize);
            Products = products;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling ((double)totalCount / pageSize);

            return Page ();
        }
    }
}
