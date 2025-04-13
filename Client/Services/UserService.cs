using ShopLib;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client.Services;

public class UserService {

    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService (IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor) {
        _httpClient = clientFactory.CreateClient ("ApiClient");
        _contextAccessor = contextAccessor;
    }

    // Получение всех пользователей кроме главного админа
    public async Task<List<UserModel>?> GetAllUsersAsync () {

        try {

            HttpResponseMessage response = await _httpClient.GetAsync ("user/all");

            if (response.IsSuccessStatusCode) {
                List<UserModel>? users = await response.Content.ReadFromJsonAsync<List<UserModel>> ();

                return users;
            }
            else {
                throw new Exception ("Failed to load users from server.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return null;
        }
    }


    // Получение пользователя
    public async Task<UserModel?> GetUserAsync (string id) {

        try {

            HttpResponseMessage response = await _httpClient.GetAsync ($"user/{id}");

            if (response.IsSuccessStatusCode) {
                UserModel? users = await response.Content.ReadFromJsonAsync<UserModel> ();

                return users;
            }
            else {
                throw new Exception ("Failed to load user from server.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine ($"Error: {ex.Message}");
            return null;
        }
    }


    // редактирование пользователя
    public async Task<bool> EditUserAsync (UserModel model) {

        string json = JsonSerializer.Serialize (model);
        StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, "user/edit") {
            Content = content
        };

        try {
            HttpResponseMessage response = await _httpClient.SendAsync (request);

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка при редактировании пользователя : {error}");
                return false;
            }

            return true;
        }
        catch (Exception ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return false;
        }
    }



}
