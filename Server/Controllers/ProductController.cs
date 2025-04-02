using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[ApiController]
[Route("product")]
//[Authorize(Policy = "RequireAdministratorRole")]
public class ProductController : Controller {

    private readonly ProductService _productService;


    public ProductController (ProductService productService) {
        _productService = productService;
    }


    [HttpPost("add")]
    public async Task AddProductAsync ([FromBody]ProductFullModel productFullModel) {

    }


    [HttpPost("change")]
    public async Task ChangeProductAsync ([FromBody] ProductFullModel productFullModel) {

    }


    [HttpDelete("delete/{productId}")]
    public async Task DeleteProductAsync (int productId) {

    }

}

