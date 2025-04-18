

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

    // получение списка картин (постранично)
    public async Task<(List<ProductShortModel>? Products, int TotalCount)> GetPagedProductsAsync (int page, int pageSize) {
        try {
            HttpResponseMessage response = await _httpClient.GetAsync ($"store/paged?page={page}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode) {
                var result = await response.Content.ReadFromJsonAsync<PagedProductResponse> ();
                return (result?.Items ?? new List<ProductShortModel> (), result?.TotalCount ?? 0);
            }
            else {
                throw new Exception ("Failed to load paged products from server.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return (new List<ProductShortModel> (), 0);
        }
    }



    // получение списка картин (фул инфа)
    public async Task<List<ProductFullModel>?> GetAllProductFullAsync () {

        try {
            HttpResponseMessage response = await _httpClient.GetAsync ("store/all_full");

            if (response.IsSuccessStatusCode) {
                List<ProductFullModel>? products = await response.Content.ReadFromJsonAsync<List<ProductFullModel>> ();
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


    // получение списка картин (фул инфа)(постранично)
    public async Task<(List<ProductFullModel>? Products, int TotalCount)> GetPagedProductFullAsync (int page, int pageSize) {
        try {
            HttpResponseMessage response = await _httpClient.GetAsync ($"store/full_paged?page={page}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode) {
                var result = await response.Content.ReadFromJsonAsync<PagedProductFullResponse> ();
                return (result?.Items ?? new List<ProductFullModel> (), result?.TotalCount ?? 0);
            }
            else {
                throw new Exception ("Failed to load paged full products from server.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return (new List<ProductFullModel> (), 0);
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
