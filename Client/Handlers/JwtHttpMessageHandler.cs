using System.Net.Http.Headers;

namespace Client.Handlers;

public class JwtHttpMessageHandler : DelegatingHandler {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtHttpMessageHandler (IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken) {
        string? token = _httpContextAccessor.HttpContext?.Session.GetString ("JwtToken");

        Console.WriteLine ($"Токен, полученный в JwtHttpMessageHandler: {token}");

        if (!string.IsNullOrEmpty (token)) {
            request.Headers.Authorization = new AuthenticationHeaderValue ("Bearer", token);
        }        

        return await base.SendAsync (request, cancellationToken);
    }
}