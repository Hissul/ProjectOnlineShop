using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using ShopLib;

namespace Server.Services;

public class OrderService {

    private readonly CartService _cartService;
    private readonly ProductService _productService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService (CartService cartService, ProductService productService, ApplicationDbContext context, ILogger<OrderService> logger) {
        _cartService = cartService;
        _productService = productService;
        _context = context;
        _logger = logger;
    }


    /// <summary>
    /// Оформление заказа / создание заказа
    /// </summary>
    public async Task<OrderModel> CreateOrderAsync (string userId, string phone, string address) {

        _logger.LogInformation ("Попытка создать заказ для пользователя {UserId}", userId);

        Cart? cart = await _cartService.GetCartByUserId (userId);

        if (cart == null || !cart.Items.Any()) {
            _logger.LogWarning ("Корзина пуста или не найдена для пользователя {UserId}", userId);
            return null;
        }

        Order order = new Order {
            UserId = userId,
            OrderedDate = DateTime.Now,
            PhoneNumber = phone,
            DeliveryAddress = address,
            TotalAmount = cart.Items.Sum (i => i.Product.Price * i.Quantity),
            OrderItems = cart.Items.Select (i => new OrderItem {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Product.Price,
            }).ToList ()
        };

        _context.Orders.Add (order);
        await _context.SaveChangesAsync ();

        // уменьшаем количество продукта
        foreach (OrderItem orderItem in order.OrderItems) {
            await _productService.ChangeQuantity (orderItem.ProductId, orderItem.Quantity);
            _logger.LogInformation ("Уменьшено количество товара {ProductId} на {Quantity}", orderItem.ProductId, orderItem.Quantity);
        }

 

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
        _logger.LogInformation ("Заказ {OrderId} успешно создан для пользователя {UserId}", order.Id, userId);

        

        return model;
    }

    /// <summary>
    /// Получение всех заказов (для юзера)
    /// </summary>
    public async Task<List<OrderModel>> GetUserOrdersAsync (string userId) {

        _logger.LogInformation ("Получение заказов пользователя {UserId}", userId);

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


    /// <summary>
    /// Получение полной инфы о заказе (для юзера)
    /// </summary>
    public async Task<OrderModel?> GetOrderFullInfoAsync(int orderId) {

        _logger.LogInformation ("Получение полной информации по заказу {OrderId}", orderId);

        OrderModel? order = await _context.Orders
       .Where (o => o.Id == orderId) 
       .Include(o => o.User)
       .Include (o => o.OrderItems)
       .ThenInclude (oi => oi.Product)
       .Select (o => new OrderModel {
           Id = o.Id, 
           OrderedDate = o.OrderedDate,
           TotalAmount = o.TotalAmount,
           Status = o.Status,
           UserName = o.User.FullName,
           UserEmail = o.User.Email,
           ItemModels = o.OrderItems.Select (oi => new OrderItemModel {
               Id = oi.Id,
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


    /// <summary>
    /// Получение заказов (по статусу)
    /// </summary>
    public async Task<List<OrderModel>> GetOrdersByStatusAsync (string status) {

        _logger.LogInformation ("Запрос заказов со статусом {Status}", status);

        List<OrderModel> orders = await _context.Orders
            .Where (o => o.Status == status)
            .Include(o => o.User)
            .Include (o => o.OrderItems)
            .ThenInclude (oi => oi.Product)
            .Select (o => new OrderModel { 
                Id = o.Id,
                OrderedDate = o.OrderedDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                UserName = o.User.FullName,
                UserEmail = o.User.Email,
                PhoneNumber = o.PhoneNumber,
                DeliveryAddress = o.DeliveryAddress,
                ItemModels = o.OrderItems.Select(oi => new OrderItemModel {
                    Id = oi.Id,
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
                        Image = oi.Product.Image
                    }
                }).ToList ()
            })
            .ToListAsync ();

        return orders;
    }


    /// <summary>
    /// Изменение заказа
    /// </summary>
    public async Task EditOrderAsync (int orderId, string orderStatus) {

        //Order? order = await _context.Orders.FirstOrDefaultAsync (o => o.Id == orderId);

        //if(order != null) {
        //    order.Status = orderStatus;
        //    await _context.SaveChangesAsync ();
        //}

        _logger.LogInformation ("Обновление статуса заказа {OrderId} на {Status}", orderId, orderStatus);

        Order? order = await _context.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync ();

        if(order != null) {
            order.Status = orderStatus;

            foreach(OrderItem item in order.OrderItems) {
                Product? product = await _context.Products.FindAsync (item.ProductId);

                if (product != null && product.Reserve > 0 && orderStatus == "Processing") {
                    product.Reserve -= item.Quantity;
                }
                else if(product != null && orderStatus == "Cancelled") {
                    product.StockQuantity += item.Quantity;
                }
            }

            await _context.SaveChangesAsync ();
            _logger.LogInformation ("Статус заказа {OrderId} обновлён успешно", orderId);
        }
    }


    /// <summary>
    /// Удаление заказа
    /// </summary>
    public async Task RemoveOrderItemAsync (int orderItemId, int orderId) {

        _logger.LogInformation ("Удаление позиции заказа {OrderItemId} из заказа {OrderId}", orderItemId, orderId);

        OrderItem? orderItem = await _context.OrderItems.FirstOrDefaultAsync (o => o.Id == orderItemId);

        if (orderItem != null) {
            _context.OrderItems.Remove (orderItem);

            Product? product = await _context.Products.FirstOrDefaultAsync (p => p.Id == orderItem.ProductId);
            if (product != null) { 
                product.StockQuantity += orderItem.Quantity;
                product.Reserve -= orderItem.Quantity;
            }

            Order? order = await _context.Orders.FirstOrDefaultAsync (o => o.Id == orderId);
            if (order != null) {
                order.TotalAmount -= orderItem.Price;
            }

            await _context.SaveChangesAsync ();
            _logger.LogInformation ("Позиция заказа удалена. Товары обновлены. Заказ обновлён.");
        }
    }

}
