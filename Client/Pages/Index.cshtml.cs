using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages;
public class IndexModel : PageModel {

    private readonly StoreService storeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel (StoreService storeService, ILogger<IndexModel> logger) {
        this.storeService = storeService;
        _logger = logger;
    }

    public List<ProductShortModel>? Products { get; set; }

    public async Task OnGetAsync () {
        Products = await storeService.GetAllProductAsync ();       
    }
}
