using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers;


public class AuthController : Controller {

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public AuthController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config) {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _config = config;
    }

    [HttpPost ("register")]
    public async Task<IActionResult> Register ([FromBody] RegisterModel model) {

        ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        IdentityResult result = await _userManager.CreateAsync (user, model.Password);

        if (!result.Succeeded)
            return BadRequest (result.Errors);

        if (!await _roleManager.RoleExistsAsync ("User")) 
            await _roleManager.CreateAsync (new IdentityRole ("User"));        

        await _userManager.AddToRoleAsync (user, "User");

        return Ok (new { message = "Пользователь зарегистрирован!" });
    }



    [HttpPost ("login")]
    public async Task<ActionResult<UserModel>> Login ([FromBody] LoginModel model) {

        if (model == null || string.IsNullOrEmpty (model.Email) || string.IsNullOrEmpty (model.Password))
            return BadRequest (new { message = "Некорректные данные" });

        ApplicationUser? user = await _userManager.FindByEmailAsync (model.Email);

        if (user == null)
            return Unauthorized (new { message = "Неверные учетные данные" });

        // попытка аутентификации пользователя 
        var result = await _signInManager.PasswordSignInAsync (user, model.Password, false, false);

        if (!result.Succeeded)
            return Unauthorized (new { message = "Неверные учетные данные" });        

        string token = GenerateJwtToken (user);

        IList<string> roles = await _userManager.GetRolesAsync (user);

        UserModel userModel = new UserModel { Email = user.Email, Token = token, Roles = roles };

        return Ok ( userModel);       
    }


    private string GenerateJwtToken (ApplicationUser user) {
        byte[] key = Encoding.UTF8.GetBytes (_config["Jwt:Secret"]);

        Claim[] claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken (
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours (1),
            signingCredentials: new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler ().WriteToken (token);
    }


    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync (bool param) {
        await _signInManager.SignOutAsync ();
        return Ok ();
    }


}
