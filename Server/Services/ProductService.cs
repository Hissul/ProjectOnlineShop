using Server.Data;
using Server.Data.Entities;
using ShopLib;

namespace Server.Services;

public class ProductService {

    private readonly ApplicationDbContext _context;

    public ProductService (ApplicationDbContext context) {
        _context = context;
    }


    /// <summary>
    /// Изменение количества продукта при заказе
    /// </summary>
    public async Task ChangeQuantity (int productId, int quantity) {
        Product? product = await _context.Products.FindAsync (productId);

        if (product != null && product.StockQuantity > 0) {
            product.StockQuantity -= quantity;

            await _context.SaveChangesAsync ();
        }        
    }

    /// <summary>
    /// Добавление нового продукта
    /// </summary>
    public async Task AddProductAsync (ProductFullModel productFullModel) {

    }


    /// <summary>
    /// Изменение продукта
    /// </summary>
    public async Task ChangeProductAsync (ProductFullModel productFullModel) {

    }


    /// <summary>
    /// Удаление продукта
    /// </summary>
    public async Task DeleteProductAsync (int productId) {

    }



}
