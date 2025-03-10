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

    public CartService (HttpClient httpClient, IHttpContextAccessor httpContextAccessor) {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    // Добавление в корзину
    public async Task AddToCartAsync (string userId, int productId) {
        
        string json = JsonSerializer.Serialize (new { UserID = userId, ProductId = productId });
        StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

        try {
            HttpResponseMessage response = await _httpClient.PostAsync ("cart/add", content);

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка при добавлении в корзину: {error}");
                return;
            }

            Console.WriteLine ($"Товар {productId} был добавлен в корзину");
        }
        catch (Exception ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return;
        }
    }

    // Получение CartItem
    public async Task<List<ItemInCart>?> GetUserCartAsync (string userId) {

        List<ItemInCart>? items = [];

        try {
            HttpResponseMessage response = await _httpClient.GetAsync ($"cart/user_cart/{userId}");

            if (response.IsSuccessStatusCode) {
                items = await response.Content.ReadFromJsonAsync<List<ItemInCart>> ();

                return items;
            }
            else {
                throw new Exception ("Failed to load cart items from server.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return items;
        }
    }


}
