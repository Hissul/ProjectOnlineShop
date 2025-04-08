using Microsoft.AspNetCore.Mvc;

using Server.Services;
using ShopLib;

namespace Server.Controllers;

[Route ("store")]
[ApiController]
public class StoreController : Controller {

    private readonly StoreService productService;

    public StoreController (StoreService productService) {
        this.productService = productService;
    }


    [HttpGet ("all")]
    public async Task<List<ProductShortModel>> GetAllProductAsync () { 
        List<ProductShortModel> products = await productService.GetAllProductAsync ();
        return products;
    }

    [HttpGet("all_full")]
    public async Task<List<ProductFullModel>> GetAllProductFullAsync () {
        var products = await productService.GetAllProductFullAsync ();
        return products;
    }

    [HttpGet("full_info/{id:int}")]
    public async Task<ProductFullModel?> GetFullProductAsync (int id) { 
        ProductFullModel? product = await productService.GetProductFullAsync (id);
        return product;
    }
}
