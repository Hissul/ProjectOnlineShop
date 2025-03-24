
using ShopLib;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Services;

public class LoginService {

    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginService (IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor) {
        _httpClient = clientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }


    // Логин
    public async Task<UserModel?> LoginAsync (string email, string password) {

        try {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync ("auth/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка авторизации: {error}");
                return null;
            }

            UserModel? user = await response.Content.ReadFromJsonAsync<UserModel> ();

            if (user == null || string.IsNullOrEmpty (user.Token))
                return null;

            //SetToken (user.Token);
            //
            // Сохраняем токен в сессии
            _httpContextAccessor.HttpContext?.Session.SetString ("JwtToken", user.Token);
            Console.WriteLine ($"Токен сохранен в сессию: {user.Token}");

            return user;
        }
        catch (HttpRequestException ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return null;
        }
    }

    //// Добавление JWT токена в заголовки
    //public void SetToken(string token) {
    //    if (!string.IsNullOrEmpty (token)) {
    //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", token);
    //        Console.WriteLine ($"Токен установлен: {_httpClient.DefaultRequestHeaders.Authorization}");
    //    }
    //}


    // Регистрация   
    public async Task<bool> RegisterAsync (string email, string password, string fullName) {
        try {
            HttpResponseMessage response = 
                await _httpClient.PostAsJsonAsync ("auth/register", 
                new { Email = email, Password = password, FullName = fullName });

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка регистрации: {error}");
                return false;
            }

            return true;
        }
        catch (HttpRequestException ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return false;
        }
    }


    // Логаут
    public async Task<bool> LogoutAsync (bool param) {

        try {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync ("auth/logout", true);

            if (!response.IsSuccessStatusCode) {
                string error = await response.Content.ReadAsStringAsync ();
                Console.WriteLine ($"Ошибка : {error}");
                return false;
            }

            return true;
        }
        catch (HttpRequestException ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return false;
        }
      
    }


}
