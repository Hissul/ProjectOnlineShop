using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Services;
using ShopLib;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers;

[Route ("auth")]
[ApiController]
public class AuthController : Controller {

    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController (AuthService authService, ILogger<AuthController> logger) {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost ("register")]
    public async Task<IActionResult> Register ([FromBody] RegisterModel model) {

        IdentityResult result = await _authService.RegisterAsync (model);

        if (!result.Succeeded) {
            _logger.LogWarning ("Ошибка регистрации через контроллер. Email: {Email}", model.Email);
            return BadRequest (result.Errors);
        }

        _logger.LogInformation ("Регистрация прошла успешно. Email: {Email}", model.Email);
        return Ok (new { message = "Пользователь зарегистрирован!" });
    }




    [HttpPost ("login")]
    public async Task<ActionResult<UserModel>> Login ([FromBody] LoginModel model) {

        if (model == null || string.IsNullOrEmpty (model.Email) || string.IsNullOrEmpty (model.Password)) {
            _logger.LogWarning ("Попытка входа с пустыми данными.");
            return BadRequest (new { message = "Некорректные данные" });
        }

        UserModel? loginResult = await _authService.LoginAsync (model);

        if (loginResult == null) {
            _logger.LogWarning ("Неверный логин: {Email}", model.Email);
            return Unauthorized (new { message = "Неверный email или пароль" });
        }

        _logger.LogInformation ("Успешный логин: {Email}", model.Email);
        return Ok (loginResult);
    }


    [HttpPost ("logout")]
    public async Task<IActionResult> LogoutAsync (bool param) {
        _logger.LogInformation ("Запрос на выход из системы");
        await _authService.LogoutAsync ();
        return Ok (new { message = "Выход выполнен успешно" });
    }



}
