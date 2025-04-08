using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using ShopLib;

namespace Server.Services;

public class OrderService {

    private readonly CartService _cartService;
    private readonly ProductService _productService;
    private readonly ApplicationDbContext _context;

    public OrderService (CartService cartService, ProductService productService, ApplicationDbContext context) {
        _cartService = cartService;
        _productService = productService;
        _context = context;
    }


    /// <summary>
    /// Оформление заказа / создание заказа
    /// </summary>
    public async Task<OrderModel> CreateOrderAsync (string userId, string phone, string address) {
       Cart? cart = await _cartService.GetCartByUserId (userId);

        if (cart == null || !cart.Items.Any()) {
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
        }

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
    /// Получение всех заказов (для юзера)
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


    /// <summary>
    /// Получение полной инфы о заказе (для юзера)
    /// </summary>
    public async Task<OrderModel?> GetOrderFullInfoAsync(int orderId) {

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



    ///// <summary>
    ///// Получение заказа (по Id)
    ///// </summary>
    //public async Task<OrderModel> GetOrdersByIdAsync (string status) { 
    
    //}



    /// <summary>
    /// Изменение заказа
    /// </summary>
    public async Task EditOrderAsync (int orderId, string orderStatus) {

        //Order? order = await _context.Orders.FirstOrDefaultAsync (o => o.Id == orderId);

        //if(order != null) {
        //    order.Status = orderStatus;
        //    await _context.SaveChangesAsync ();
        //}

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
            }

            await _context.SaveChangesAsync ();
        }
    }


    /// <summary>
    /// Удаление заказа
    /// </summary>
    public async Task RemoveOrderItemAsync (int orderItemId, int orderId) {

        OrderItem? orderItem = await _context.OrderItems.FirstOrDefaultAsync (o => o.Id == orderItemId);        

        if (orderItem != null) { 
            _context.OrderItems.Remove (orderItem);

            Order? order = await _context.Orders.FirstOrDefaultAsync (o => o.Id == orderId);

            if (order != null) {
                order.TotalAmount -= orderItem.Price;
            }

            await _context.SaveChangesAsync ();
        }
    }

}
