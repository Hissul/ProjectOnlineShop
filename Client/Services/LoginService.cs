using Client.Models;
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

    public LoginService (HttpClient httpClient, IHttpContextAccessor httpContextAccessor) {
        _httpClient = httpClient;
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

            SetToken (user.Token);

            return user;
        }
        catch (HttpRequestException ex) {
            Console.WriteLine ($"Ошибка сети: {ex.Message}");
            return null;
        }
    }

    //public async Task<bool> LoginAsync (string email, string password) {
    //    var loginModel = new { Email = email, Password = password };
    //    var response = await _httpClient.PostAsJsonAsync ("api/auth/login", loginModel);

    //    if (!response.IsSuccessStatusCode)
    //        return false;

    //    var result = await response.Content.ReadFromJsonAsync<UserModel> ();
    //    if (result == null || string.IsNullOrEmpty (result.Token))
    //        return false;

    //    // Сохраняем токен в localStorage
    //    await SecureStorage.SetAsync ("auth_token", result.Token);

    //    // Извлекаем роль из JWT
    //    var handler = new JwtSecurityTokenHandler ();
    //    var jwt = handler.ReadJwtToken (result.Token);
    //    var role = jwt.Claims.FirstOrDefault (c => c.Type == ClaimTypes.Role)?.Value;

    //    if (!string.IsNullOrEmpty (role)) {
    //        await SecureStorage.SetAsync ("user_role", role);
    //    }

    //    return true;
    //}


    // Добавление JWT токена в заголовки
    public void SetToken(string token) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", token);
    }


    // Регистрация   
    public async Task<bool> RegisterAsync (string email, string password) {
        try {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync ("auth/register", new { Email = email, Password = password });

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
