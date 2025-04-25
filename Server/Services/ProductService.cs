using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using ShopLib;

namespace Server.Services;

public class ProductService {

    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductService> _logger;


    public ProductService (ApplicationDbContext context, ILogger<ProductService> logger) {
        _context = context;
        _logger = logger;
    }


    /// <summary>
    /// Изменение количества продукта при заказе
    /// </summary>
    public async Task ChangeQuantity (int productId, int quantity) {
        Product? product = await _context.Products.FindAsync (productId);

        if (product != null && product.StockQuantity > 0) {
            product.StockQuantity -= quantity;

            if (product.Reserve == null)
                product.Reserve = 0;

            product.Reserve += quantity;

            await _context.SaveChangesAsync ();

            _logger.LogInformation ("Изменено количество товара (ID: {ProductId}): -{Quantity}, резерв теперь: {Reserve}", productId, quantity, product.Reserve);
        }
        else {
            _logger.LogWarning ("Не удалось изменить количество товара (ID: {ProductId}). Товар не найден или недостаточное количество.", productId);
        }
    }


    /// <summary>
    /// Добавление нового продукта
    /// </summary>
    public async Task AddProductAsync (ProductFullModel productFullModel) {
        Product product = new Product {
            Name = productFullModel.Name,
            Description = productFullModel.Description,
            Price = productFullModel.Price,
            Image = productFullModel.Image,
            StockQuantity = productFullModel.StockQuantity,
            CreatedAt = DateTime.Now,
            Reserve = productFullModel.Reserve ?? 0
        };

        product.ProductInfo = new ProductInfo {
            Technique = productFullModel.Technique,
            Material = productFullModel.Material,
            Plot = productFullModel.Plot,
            Style = productFullModel.Style,
            Height = productFullModel.Height,
            Wight = productFullModel.Wight,
            Year = productFullModel.Year
        };

        _context.Products.Add (product);
        await _context.SaveChangesAsync ();

        _logger.LogInformation ("Добавлен товар: {ProductName}, ID: {ProductId}", product.Name, product.Id);
    }



    /// <summary>
    /// Изменение продукта
    /// </summary>
    public async Task EditProductAsync (ProductFullModel productFullModel) {
        Product? product = await _context.Products
            .Where (p => p.Id == productFullModel.Id)
            .Include (p => p.ProductInfo)
            .FirstOrDefaultAsync ();

        if (product != null) {
            product.Name = productFullModel.Name;
            product.Description = productFullModel.Description;
            product.Price = productFullModel.Price;
            product.StockQuantity = productFullModel.StockQuantity;
            product.Reserve = productFullModel.Reserve;
            product.Image = productFullModel.Image;

            product.ProductInfo.Technique = productFullModel.Technique;
            product.ProductInfo.Material = productFullModel.Material;
            product.ProductInfo.Plot = productFullModel.Plot;
            product.ProductInfo.Style = productFullModel.Style;
            product.ProductInfo.Year = productFullModel.Year;
            product.ProductInfo.Wight = productFullModel.Wight;
            product.ProductInfo.Height = productFullModel.Height;

            await _context.SaveChangesAsync ();

            _logger.LogInformation ("Обновлен товар: {ProductName}, ID: {ProductId}", product.Name, product.Id);
        }
        else {
            _logger.LogWarning ("Товар для редактирования не найден: ID {ProductId}", productFullModel.Id);
        }
    }


    /// <summary>
    /// Удаление продукта
    /// </summary>
    public async Task DeleteProductAsync (int productId) {
        var product = await _context.Products
            .Include (p => p.ProductInfo)
            .FirstOrDefaultAsync (p => p.Id == productId);

        if (product != null) {
            _context.ProductInfos.Remove (product.ProductInfo);
            _context.Products.Remove (product);
            await _context.SaveChangesAsync ();

            _logger.LogInformation ("Удален товар: {ProductName}, ID: {ProductId}", product.Name, product.Id);
        }
        else {
            _logger.LogWarning ("Не удалось удалить товар. Товар с ID {ProductId} не найден.", productId);
        }
    }
}
