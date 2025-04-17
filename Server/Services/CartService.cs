using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using ShopLib;

namespace Server.Services;

public class CartService {

    private readonly ApplicationDbContext _context;

    public CartService (ApplicationDbContext context) {
        _context = context;
    }

    /// <summary>
    /// Получение корзины по UserId
    /// </summary>
    public async Task<Cart?> GetCartByUserId (string userId) {
        return await _context.Carts
            .Include (c => c.Items)
            .ThenInclude (i => i.Product)
            .FirstOrDefaultAsync (c => c.UserId == userId);
    }

    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    public async Task AddToCartAsync (string userId, int productId, int quantity = 1) {

        Cart? cart = await GetCartByUserId (userId);

        if (cart == null) {
            cart = new Cart { UserId = userId };

            _context.Carts.Add (cart);
            await _context.SaveChangesAsync ();

            // LOGER
        }

        CartItem? cartItem = cart.Items.FirstOrDefault (i => i.ProductId == productId);

        if (cartItem != null) {
            cartItem.Quantity += quantity;
        }
        else {
            cart.Items.Add (new CartItem { ProductId = productId, Quantity = quantity });
        }

        await _context.SaveChangesAsync ();

        // LOGER
    }


    /// <summary>
    /// Получение CartItems по UserId
    /// </summary>
    public async Task<List<ItemInCartModel>> GetCartItemsByUserIdAsync (string userId) {

        Cart? cart = await GetCartByUserId (userId);

        if (cart == null) {
            cart= new Cart { UserId = userId };

            _context.Carts.Add (cart);
            await _context.SaveChangesAsync ();

            // LOGER
        }

        List<ItemInCartModel> items = [];

        foreach (CartItem item in cart.Items) {
            items.Add (new ItemInCartModel {
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
            });
        }

        // LOGER

        return items;
    }


    /// <summary>
    /// Удаление товара из корзины
    /// </summary>
    public async Task RemoveFromCartAsync(string userId, int productId) {
        Cart? cart = await GetCartByUserId (userId);

        if (cart != null) {
            CartItem? cartItem = cart.Items.FirstOrDefault (c => c.ProductId == productId);            

            cart.Items.Remove (cartItem);
            await _context.SaveChangesAsync ();

            // LOGER
        }
    }


    /// <summary>
    /// Очистка корзины
    /// </summary>
    public async Task ClearCartAsync (string userId) {
        Cart? cart = await GetCartByUserId (userId);

        if(cart != null) {
            _context.CartItems.RemoveRange (cart.Items);
            await _context.SaveChangesAsync ();

            // LOGER
        }
    }

}
