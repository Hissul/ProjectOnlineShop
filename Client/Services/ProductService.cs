using Microsoft.AspNetCore.Mvc;
using ShopLib;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text;

namespace Client.Services;

public class ProductService {

    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductService (IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) {
        _httpClient = clientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }


    // редактирование продукта
    public async Task<bool> EditProductAsync (ProductFullModel product) {

        string json = JsonSerializer.Serialize (product);
        StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, "product/edit") {
            Content = content
        };

        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка при изменении товара : {error}");
                return false;
            }
            Console.WriteLine ($"Товар {product.Id} был изменен");
            return true;
        }
        catch (Exception ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return false;
        }
    }

    // добавление продукта
    public async Task<bool> AddProductAsync (ProductFullModel product) {

        string json = JsonSerializer.Serialize (product);

        Console.WriteLine (json);

        StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, "product/add") {
            Content = content
        };

        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка при добавлении товара : {error}");
                return false;
            }
            Console.WriteLine ($"Товар был добавлен!");
            return true;
        }
        catch (Exception ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return false;
        }

    }




}
