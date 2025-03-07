using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Models;
using Server.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers;

[Route ("auth")]
[ApiController]
public class AuthController : Controller {

    private readonly AuthService _authService;

    public AuthController (AuthService authService) {
        _authService = authService;
    }

    [HttpPost ("register")]
    public async Task<IActionResult> Register ([FromBody] RegisterModel model) {

        IdentityResult result = await _authService.RegisterAsync (model);

        if (!result.Succeeded) 
            return BadRequest (result.Errors);
        

        return Ok (new { message = "Пользователь зарегистрирован!" });
    }




    [HttpPost ("login")]
    public async Task<ActionResult<UserModel>> Login ([FromBody] LoginModel model) {

        if (model == null || string.IsNullOrEmpty (model.Email) || string.IsNullOrEmpty (model.Password))
            return BadRequest (new { message = "Некорректные данные" });

        UserModel? loginResult = await _authService.LoginAsync (model);

        if(loginResult == null)
            return Unauthorized (new { message = "Неверный email или пароль" });

        return Ok (loginResult);
    }


    [HttpPost ("logout")]
    public async Task<IActionResult> LogoutAsync (bool param) {
        await _authService.LogoutAsync ();
        return Ok (new { message = "Выход выполнен успешно" });
    }



}
