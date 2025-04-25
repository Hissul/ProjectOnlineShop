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
    private readonly ILogger<ProductController> _logger;


    public ProductController (ProductService productService, ILogger<ProductController> logger) {
        _productService = productService;
        _logger = logger;
    }


    [HttpPost ("add")]
    public async Task<IActionResult> AddProductAsync ([FromBody] ProductFullModel productFullModel) {
        if (!ModelState.IsValid) {
            _logger.LogWarning ("Некорректная модель при добавлении товара.");
            return BadRequest (ModelState);
        }

        await _productService.AddProductAsync (productFullModel);
        _logger.LogInformation ("Товар успешно добавлен: {ProductName}", productFullModel.Name);
        return Ok (new { message = "Товар добавлен!" });
    }


    [HttpPost ("edit")]
    public async Task<IActionResult> EditProductAsync ([FromBody] ProductFullModel productFullModel) {
        _logger.LogInformation ("Запрос на редактирование товара: ID {ProductId}", productFullModel.Id);
        await _productService.EditProductAsync (productFullModel);
        return Ok (new { message = "Товар изменен!" });
    }


    [HttpDelete ("delete/{productId}")]
    public async Task<IActionResult> DeleteProductAsync (int productId) {
        _logger.LogInformation ("Запрос на удаление товара: ID {ProductId}", productId);
        await _productService.DeleteProductAsync (productId);
        return Ok (new { message = "Товар удален!" });
    }

}

