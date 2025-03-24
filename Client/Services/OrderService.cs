using ShopLib;
using System.Reflection;

namespace Client.Services;

public class OrderService {

    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService (IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) {
        _httpClient = clientFactory.CreateClient ("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }


    // Формирование заказа
    public async Task<OrderModel?> CreateOrderAsync (string userId) {

        OrderModel? model = null;
        //Console.WriteLine ($"BaseAddress: {_httpClient.BaseAddress}");

        try {
            HttpResponseMessage response = await _httpClient.GetAsync ($"order/create/{userId}");

            if (response.IsSuccessStatusCode) {
                model = await response.Content.ReadFromJsonAsync<OrderModel> ();
                return model;
            }
            else {
                throw new Exception ("Failed to create order");
            }
        }
        catch (Exception ex) { 
            Console.WriteLine ($"Error: {ex.Message}");
            return model;
        }
    }

    // Получение списка заказов для пользователя
    public async Task<List<OrderModel>?> GetUserOrdersAsync (string userId) {

        List<OrderModel>? orderModels = null;
        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Get, $"order/all/{userId}");

        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            if (response.IsSuccessStatusCode) {
                orderModels = await response.Content.ReadFromJsonAsync<List<OrderModel>> ();
                return orderModels;
            }
            else {
                throw new Exception ("Failed to get orders");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return orderModels;
        }
    }


    // Получение фулл инфы о заказе
    public async Task<OrderModel?> GetOrderFullInfoAsync (int orderId) {

        OrderModel? orderModel = null;
        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Get, $"order/info/{orderId}");

        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            if (response.IsSuccessStatusCode) {

                orderModel = await response.Content.ReadFromJsonAsync<OrderModel> ();
                return orderModel;
            }
            else {
                throw new Exception ("Failed to get order info");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return orderModel;
        }
    }


}
