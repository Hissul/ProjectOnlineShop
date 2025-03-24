using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using ShopLib;

namespace Server.Services;

public class OrderService {

    private readonly CartService _cartService;
    private readonly ApplicationDbContext _context;

    public OrderService (CartService cartService, ApplicationDbContext context) {
        _cartService = cartService;
        _context = context;
    }


    /// <summary>
    /// Оформление заказа
    /// </summary>
    public async Task<OrderModel> CreateOrderAsync (string userId) {
       Cart? cart = await _cartService.GetCartByUserId (userId);

        if (cart == null || !cart.Items.Any()) {
            return null;
        }

        Order order = new Order {
            UserId = userId,
            OrderedDate = DateTime.Now,
            TotalAmount = cart.Items.Sum (i => i.Product.Price * i.Quantity),
            OrderItems = cart.Items.Select (i => new OrderItem {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Product.Price,
            }).ToList ()
        };

        _context.Orders.Add (order);
        await _context.SaveChangesAsync ();

        // LOGER

        OrderModel model = new OrderModel {
            Id = order.Id,
            OrderedDate = order.OrderedDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ItemModels = order.OrderItems.Select (i => new OrderItemModel {
                Quantity = i.Quantity,
                Price = i.Product.Price,
                ProductId = i.ProductId,
                OrderId = i.OrderId,
            }).ToList ()
        };

        await _cartService.ClearCartAsync (userId);

        // LOGER

        return model;
    }

    /// <summary>
    /// Получение всех заказов
    /// </summary>
    public async Task<List<OrderModel>> GetUserOrdersAsync (string userId) {

        List<OrderModel> userOrders = await _context.Orders
            .Where (i => i.UserId == userId)           
            .OrderByDescending(o => o.OrderedDate)
            .Select(o => new OrderModel {
                Id = o.Id,
                OrderedDate = o.OrderedDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status                
            }).ToListAsync ();

        return userOrders;
    }


    public async Task<OrderModel?> GetOrderFullInfoAsync(int orderId) {

        OrderModel? order = await _context.Orders
            .Include (o => o.OrderItems)
            .ThenInclude (oi => oi.Product)
            .Select (o => new OrderModel {
                Id = orderId,
                OrderedDate = o.OrderedDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                ItemModels = o.OrderItems.Select (o => new OrderItemModel {
                    Quantity = o.Quantity,
                    Price = o.Price,
                    OrderId = orderId,
                    ProductId = o.ProductId,
                    Product = new ProductShortModel {
                        Id = o.Product.Id,
                        Name = o.Product.Name,
                        Description = o.Product.Description,
                        Price = o.Product.Price,
                        StockQuantity = o.Product.StockQuantity,
                        Image = o.Product.Image,
                    }
                }).ToList ()
            })
            .FirstOrDefaultAsync (o => o.Id == orderId);

        return order;
    }
}
