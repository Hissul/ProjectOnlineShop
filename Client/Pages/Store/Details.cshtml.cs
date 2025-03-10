
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Store
{
    public class DetailsModel : PageModel
    {

        private readonly StoreService storeService;

        public DetailsModel (StoreService storeService) {
            this.storeService = storeService;
        }

        public ProductFullModel Product { get; set; }

        public async Task OnGet(int productId){
            Product = await storeService.GetFullProductAsync(productId);
        }
    }
}
