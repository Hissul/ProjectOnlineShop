using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Admin.Products
{
    public class AddProductModel : PageModel
    {
        private readonly StoreService _storeService;
        private readonly ProductService _productService;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddProductModel (StoreService storeService, ProductService productService, IHttpContextAccessor contextAccessor) {
            _storeService = storeService;
            _productService = productService;
            _contextAccessor = contextAccessor;
        }

        [BindProperty]
        public ProductFullModel Product { get; set; }    

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public void OnGet(){
        }

        public async Task<IActionResult> OnPostAsync () {
      
            if(ImageFile != null && ImageFile.Length > 0) {
                using MemoryStream memoryStream = new MemoryStream ();
                await ImageFile.CopyToAsync (memoryStream);
                Product.Image = memoryStream.ToArray ();
            }

            Product.Size = $"{Product.Wight} x {Product.Height}";

            bool result = await _productService.AddProductAsync (Product);

            if (result) {
                return RedirectToPage ("/Admin/Products/ChangeProduct");
            }
          
            return Page();
        }

    }
}
