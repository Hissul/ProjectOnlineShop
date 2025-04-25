using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using ShopLib;

namespace Server.Services;

public class CartService {

    private readonly ApplicationDbContext _context;
    private readonly ILogger<CartService> _logger;


    public CartService (ApplicationDbContext context, ILogger<CartService> logger) {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Получение корзины по UserId
    /// </summary>
    public async Task<Cart?> GetCartByUserId (string userId) {
 _logger.LogInformation("Запрос корзины для пользователя: {UserId}", userId);

    return await _context.Carts
        .Include(c => c.Items)
        .ThenInclude(i => i.Product)
        .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    public async Task AddToCartAsync (string userId, int productId, int quantity = 1) {

        _logger.LogInformation ("Добавление товара {ProductId} (кол-во: {Quantity}) в корзину пользователя {UserId}", productId, quantity, userId);

        Cart? cart = await GetCartByUserId (userId);

        if (cart == null) {
            cart = new Cart { UserId = userId };
            _context.Carts.Add (cart);
            await _context.SaveChangesAsync ();
            _logger.LogInformation ("Создана новая корзина для пользователя {UserId}", userId);
        }

        CartItem? cartItem = cart.Items.FirstOrDefault (i => i.ProductId == productId);

        if (cartItem != null) {
            cartItem.Quantity += quantity;
            _logger.LogInformation ("Обновлено количество товара {ProductId} в корзине", productId);
        }
        else {
            cart.Items.Add (new CartItem { ProductId = productId, Quantity = quantity });
            _logger.LogInformation ("Добавлен новый товар {ProductId} в корзину", productId);
        }

        await _context.SaveChangesAsync ();
    }


    /// <summary>
    /// Получение CartItems по UserId
    /// </summary>
    public async Task<List<ItemInCartModel>> GetCartItemsByUserIdAsync (string userId) {
        _logger.LogInformation ("Получение товаров в корзине пользователя {UserId}", userId);

        Cart? cart = await GetCartByUserId (userId);

        if (cart == null) {
            cart = new Cart { UserId = userId };
            _context.Carts.Add (cart);
            await _context.SaveChangesAsync ();
            _logger.LogInformation ("Создана пустая корзина для пользователя {UserId}", userId);
        }

        List<ItemInCartModel> items = cart.Items.Select (item => new ItemInCartModel {
            Id = item.Id,
            Quantity = item.Quantity,
            Product = new ProductShortModel {
                Id = item.ProductId,
                Name = item.Product.Name,
                Description = item.Product.Description,
                Price = item.Product.Price,
                StockQuantity = item.Product.StockQuantity,
                Image = item.Product.Image,
            }
        }).ToList ();

        _logger.LogInformation ("Получено {Count} товаров из корзины пользователя {UserId}", items.Count, userId);

        return items;
    }


    /// <summary>
    /// Удаление товара из корзины
    /// </summary>
    public async Task RemoveFromCartAsync(string userId, int productId) {
        _logger.LogInformation ("Удаление товара {ProductId} из корзины пользователя {UserId}", productId, userId);

        Cart? cart = await GetCartByUserId (userId);

        if (cart != null) {
            CartItem? cartItem = cart.Items.FirstOrDefault (c => c.ProductId == productId);

            if (cartItem != null) {
                cart.Items.Remove (cartItem);
                await _context.SaveChangesAsync ();
                _logger.LogInformation ("Товар {ProductId} удален из корзины пользователя {UserId}", productId, userId);
            }
        }
    }


    /// <summary>
    /// Очистка корзины
    /// </summary>
    public async Task ClearCartAsync (string userId) {
        _logger.LogInformation ("Очистка корзины пользователя {UserId}", userId);

        Cart? cart = await GetCartByUserId (userId);

        if (cart != null) {
            _context.CartItems.RemoveRange (cart.Items);
            await _context.SaveChangesAsync ();
            _logger.LogInformation ("Корзина пользователя {UserId} очищена", userId);
        }
    }

}
