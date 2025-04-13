using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Admin.Products
{
    public class EditProductModel : PageModel
    {
        private readonly StoreService _storeService;
        private readonly ProductService _productService;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditProductModel (StoreService storeService, ProductService productService, IHttpContextAccessor contextAccessor) {
            _storeService = storeService;
            _productService = productService;
            _contextAccessor = contextAccessor;            
        }

        [BindProperty]
        public ProductFullModel Product { get; set; }     

        [BindProperty]
        public string? ReturnUrl { get; set; }


        public async Task<IActionResult> OnGetAsync(int productId, string returnUrl){

            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            ReturnUrl = returnUrl ?? "/Admin/Statistics/Statistic";           

            Product = await _storeService.GetFullProductAsync (productId);           
            return Page ();
        }



        public async Task<IActionResult> OnPostAsync () {

            Product.Size = $"{Product.Wight} x {Product.Height}";           

            bool result = await _productService.EditProductAsync (Product);

            
            if (result) {
                return RedirectToPage (ReturnUrl);
            }

            return Page ();
        }

    }
}
