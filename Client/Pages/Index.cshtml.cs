
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages;
public class IndexModel : PageModel {

    private readonly StoreService storeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel (StoreService storeService, ILogger<IndexModel> logger) {
        this.storeService = storeService;
        _logger = logger;
    }

    //public List<ProductShortModel>? Products { get; set; }

    //public async Task OnGetAsync () {
    //    Products = await storeService.GetAllProductAsync ();       
    //}


    public List<ProductShortModel>? Products { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 6;

    public async Task OnGetAsync (int pageNumber = 1) {
        CurrentPage = pageNumber;
        var (products, totalCount) = await storeService.GetPagedProductsAsync (pageNumber, PageSize);
        Products = products;
        TotalPages = (int)Math.Ceiling (totalCount / (double)PageSize);
    }
}
