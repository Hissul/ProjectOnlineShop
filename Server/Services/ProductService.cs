using Server.Data;
using ShopLib;

namespace Server.Services;

public class ProductService {

    private readonly ApplicationDbContext _context;

    public ProductService (ApplicationDbContext context) {
        _context = context;
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
