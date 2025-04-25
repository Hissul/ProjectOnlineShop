using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[Route ("user")]
[Controller]
[Authorize (Policy = "RequireAdministratorRole")]
public class UserController : Controller {

    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController (UserService userService, ILogger<UserController> logger) {
        _userService = userService;
        _logger = logger;
    }


    [HttpGet ("all")]
    public async Task<List<UserModel>> GetAllUsersAsync () {
        _logger.LogInformation ("Запрос на получение всех пользователей");
        return await _userService.GetAllUsersAsync ();
    }

    [HttpGet ("{id}")]
    public async Task<UserModel> GetUserAsync (string id) {
        _logger.LogInformation ("Запрос на получение пользователя ID: {UserId}", id);
        return await _userService.GetUserAsync (id);
    }

    [HttpPost ("edit")]
    public async Task<IActionResult> EditUserAsync ([FromBody] UserModel model) {
        _logger.LogInformation ("Запрос на редактирование пользователя ID: {UserId}", model.Id);
        bool result = await _userService.EditUserAsync (model);

        if (!result) {
            _logger.LogWarning ("Редактирование пользователя не удалось: {UserId}", model.Id);
            return BadRequest (new { message = "Не удалось обновить пользователя" });
        }

        return Ok (new { message = "Пользователь успешно обновлён" });
    }

}
