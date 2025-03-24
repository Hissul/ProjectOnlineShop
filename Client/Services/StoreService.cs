

using ShopLib;

namespace Client.Services;

public class StoreService {

    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StoreService (IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) {
        _httpClient = clientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }


    // получение списка картин
    public async Task<List<ProductShortModel>?> GetAllProductAsync () {

        try {
            HttpResponseMessage response = await _httpClient.GetAsync ("store/all");

            if (response.IsSuccessStatusCode) {
                List<ProductShortModel>? products = await response.Content.ReadFromJsonAsync<List<ProductShortModel>> ();
                return products;
            }
            else {
                throw new Exception ("Failed to load products from server.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return null;
        }        
    }


    // фуловая информация о картине
    public async Task<ProductFullModel?> GetFullProductAsync (int id) {
        try {
            HttpResponseMessage response = await _httpClient.GetAsync ($"store/full_info/{id}");

            if (response.IsSuccessStatusCode) {
                ProductFullModel? product = await response.Content.ReadFromJsonAsync<ProductFullModel> ();
                return product;
            }
            else {
                throw new Exception ("Failed to load product full info from server.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return null;
        }
    }

}
