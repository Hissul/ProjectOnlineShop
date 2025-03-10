using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using ShopLib;


namespace Server.Services;

public class StoreService {

    private readonly ApplicationDbContext _context;

    public StoreService (ApplicationDbContext context) {
        _context = context;
    }

    /// <summary>
    /// Получение всех продуктов
    /// </summary>
    public async Task<List<ProductShortModel>> GetAllProductAsync () {
        List<ProductShortModel> res = await _context.Products
            .Select (p => new ProductShortModel {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Image = p.Image,
            }).ToListAsync ();

        return res;
    }

    /// <summary>
    /// Получение полной информации о продукте
    /// </summary>
    public async Task<ProductFullModel?> GetProductFullAsync (int id) {   

        ProductFullModel? product = await _context.Products
           .Where (p => p.Id == id)
           .Include (pr => pr.ProductInfo)
           .Select(p => new ProductFullModel {
               Id = p.Id,
               Name = p.Name,
               Description = p.Description,
               Price = p.Price,
               StockQuantity = p.StockQuantity,
               Image = p.Image,
               Technique = p.ProductInfo != null ? p.ProductInfo.Technique : "Не указано",
               Material = p.ProductInfo != null ? p.ProductInfo.Material : "Не указано",
               Plot = p.ProductInfo != null ? p.ProductInfo.Plot : "Не указано",
               Style = p.ProductInfo != null ? p.ProductInfo.Style : "Не указано",
               Size = p.ProductInfo != null ? $"{p.ProductInfo.Wight} × {p.ProductInfo.Height}" : "Не указано",
               Year = p.ProductInfo != null ? p.ProductInfo.Year : 0
           }).FirstOrDefaultAsync ();

        return product;
    }
}
