using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Store
{
    public class DetailsModel : PageModel
    {

        private readonly StoreService storeService;

        public DetailsModel (StoreService storeService) {
            this.storeService = storeService;
        }

        public ProductFullModel Product { get; set; }

        public async Task OnGet(int id){
            Product = await storeService.GetFullProductAsync(id);
        }
    }
}
