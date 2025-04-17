using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("all_full")]
    public async Task<List<ProductFullModel>> GetAllProductFullAsync () {
        var products = await _storeService.GetAllProductFullAsync ();
        return products;
    }

    [HttpGet("full_info/{id:int}")]
    public async Task<ProductFullModel?> GetFullProductAsync (int id) { 
        ProductFullModel? product = await _storeService.GetProductFullAsync (id);
        return product;
    }
}
