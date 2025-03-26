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
       .Where (o => o.Id == orderId) // Сначала фильтруем по Id
       .Include (o => o.OrderItems)
       .ThenInclude (oi => oi.Product)
       .Select (o => new OrderModel {
           Id = o.Id, // Берем ID из базы
           OrderedDate = o.OrderedDate,
           TotalAmount = o.TotalAmount,
           Status = o.Status,
           ItemModels = o.OrderItems.Select (oi => new OrderItemModel {
               Quantity = oi.Quantity,
               Price = oi.Price,
               OrderId = oi.OrderId,
               ProductId = oi.ProductId,
               Product = new ProductShortModel {
                   Id = oi.Product.Id,
                   Name = oi.Product.Name,
                   Description = oi.Product.Description,
                   Price = oi.Product.Price,
                   StockQuantity = oi.Product.StockQuantity,
                   Image = oi.Product.Image,
               }
           }).ToList ()
       })
       .FirstOrDefaultAsync ();

        return order;
    }
}
