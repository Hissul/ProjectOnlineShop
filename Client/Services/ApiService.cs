using Client.Models;
using System.Net.Http.Headers;

namespace Client.Services;

public class ApiService {

    private readonly HttpClient _httpClient;

    public ApiService (HttpClient httpClient) {
        _httpClient = httpClient;
    }


    // Логин
    public async Task<string> LoginAsync (string email, string password) {

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync ("login", new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
            return null;

        AuthResponse? result = await response.Content.ReadFromJsonAsync<AuthResponse> ();

        return result?.Token;
    }


    // Добавление JWT токена в заголовки
    public void SetToken(string token) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", token);
    }


    // Регистрация
    public async Task<string> RegisterAsync (string email, string password) {

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync ("register", new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
            return null;

        return "Пользователь успешно зарегистрирован!";
    }


}
