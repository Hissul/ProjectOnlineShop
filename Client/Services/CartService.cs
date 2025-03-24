using Microsoft.AspNetCore.Http.HttpResults;
using ShopLib;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Services;

public class CartService {

    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor   _httpContextAccessor;

    public CartService (IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) {
        _httpClient = clientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }

    // Добавление в корзину
    public async Task<bool> AddToCartAsync (string userId, int productId) {  

        string json = JsonSerializer.Serialize (new { UserID = userId, ProductId = productId });
        StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, "cart/add") {
            Content = content
        };
   
        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            Console.WriteLine ($"Заголовки запроса: {string.Join (", ", request.Headers.Select (h => $"{h.Key}: {string.Join (";", h.Value)}"))}");

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка при добавлении в корзину: {error}");
                return false;
            }

            Console.WriteLine ($"Товар {productId} был добавлен в корзину");

            return true;
        }
        catch (Exception ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return false;
        }
    }

    // Получение CartItem
    public async Task<List<ItemInCartModel>?> GetUserCartAsync (string userId) {      

        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Get, $"cart/user_cart/{userId}");

        //Console.WriteLine ($"BaseAddress: {_httpClient.BaseAddress}");

        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            Console.WriteLine ($"Заголовки запроса: {string.Join (", ", request.Headers.Select (h => $"{h.Key}: {string.Join (";", h.Value)}"))}");

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<List<ItemInCartModel>> ();
            }
            else {
                throw new Exception ("Не удалось загрузить товары в корзине.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Ошибка: {ex}");
            return new List<ItemInCartModel> ();
        }     


        //List<ItemInCartModel>? items = [];

        //try {
        //    HttpResponseMessage response = await _httpClient.GetAsync ($"cart/user_cart/{userId}");

        //    if (response.IsSuccessStatusCode) {
        //        items = await response.Content.ReadFromJsonAsync<List<ItemInCartModel>> ();

        //        return items;
        //    }
        //    else {
        //        throw new Exception ("Failed to load cart items from server.");
        //    }
        //}
        //catch (Exception ex) {
        //    Console.WriteLine ($"Error: {ex.Message}");
        //    return items;
        //}
    }

    // удаление товара из корзины
    public async Task RemoveProductFromCartAsync (string userId, int productId) {

        string json = JsonSerializer.Serialize (new { UserId = userId, ProductId = productId });
        StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, "cart/remove") {
            Content = content
        };

        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка при удаление товара из корзины: {error}");
                return;
            }
            Console.WriteLine ($"Товар {productId} был удален из корзины");
        }
        catch (Exception ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return;
        }
    }


}
