using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ShopLib;

namespace Server.Controllers;

[Route ("user")]
[Controller]


public class UserController : Controller {

    private readonly UserService _userService;

    public UserController (UserService userService) {
        _userService = userService;
    }


    [HttpGet ("all")]
    public async Task<List<UserModel>> GetAllUsersAsync () {
        List<UserModel> users = await _userService.GetAllUsersAsync ();
        return users;
    }


    [HttpGet ("{id}")]
    public async Task<UserModel> GetUserAsync (string id) {
        UserModel user = await _userService.GetUserAsync (id);
        return user;
    }


    [HttpPost("edit")]
    public async Task<IActionResult> EditUserAsync ([FromBody]UserModel model) {
        bool result = await _userService.EditUserAsync (model);

        if (!result) {
            return BadRequest (new { message = "Не удалось обновить пользователя" });
        }

        return Ok (new { message = "Пользователь успешно обновлён" });
    }





}
