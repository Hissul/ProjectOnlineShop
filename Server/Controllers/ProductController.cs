using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[ApiController]
[Route("product")]
[Authorize(Policy = "RequireAdministratorRole")]
public class ProductController : Controller {

    private readonly ProductService _productService;


    public ProductController (ProductService productService) {
        _productService = productService;
    }


    [HttpPost("add")]
    public async Task<IActionResult> AddProductAsync ([FromBody]ProductFullModel productFullModel) {

        if (!ModelState.IsValid) {
            return BadRequest (ModelState);
        }

        await _productService.AddProductAsync(productFullModel);
        return Ok (new { message = "Товар добавлен!" });
    }


    [HttpPost("edit")]
    public async Task<IActionResult> EditProductAsync ([FromBody] ProductFullModel productFullModel) {
        Console.WriteLine ("\n\n\nE  D  I  T\n\n");
        await _productService.EditProductAsync(productFullModel);
        return Ok(new { message = "Товар изменен!" });
    }


    [HttpDelete("delete/{productId}")]
    public async Task DeleteProductAsync (int productId) {

    }

}

