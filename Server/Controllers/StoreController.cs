using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[Route ("store")]
[ApiController]
public class StoreController : Controller {

    private readonly StoreService _storeService;

    public StoreController (StoreService storeService) {
        this._storeService = storeService;
    }


    [HttpGet ("all")]
    public async Task<List<ProductShortModel>> GetAllProductAsync () { 
        List<ProductShortModel> products = await _storeService.GetAllProductAsync ();
        return products;
    }


    [HttpGet ("paged")]
    public async Task<IActionResult> GetPagedProducts ([FromQuery] int page = 1, [FromQuery] int pageSize = 6) {
        var (products, totalCount) = await _storeService.GetPagedProductsAsync (page, pageSize);

        return Ok (new {
            Items = products,
            TotalCount = totalCount
        });
    }



    [HttpGet("all_full")]
    public async Task<List<ProductFullModel>> GetAllProductFullAsync () {
        var products = await _storeService.GetAllProductFullAsync ();
        return products;
    }


    [HttpGet ("full_paged")]
    public async Task<IActionResult> GetPagedFullProducts ([FromQuery] int page = 1, [FromQuery] int pageSize = 6) {
        var query = _storeService.GetFullProductQuery ();

        int totalCount = await query.CountAsync ();

        var products = await query
            .OrderBy (p => p.Id)
            .Skip ((page - 1) * pageSize)
            .Take (pageSize)
            .ToListAsync ();

        return Ok (new {
            Items = products,
            TotalCount = totalCount
        });
    }



    [HttpGet("full_info/{id:int}")]
    public async Task<ProductFullModel?> GetFullProductAsync (int id) { 
        ProductFullModel? product = await _storeService.GetProductFullAsync (id);
        return product;
    }

}
