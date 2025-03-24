using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using ShopLib;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services;

public class AuthService {

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public AuthService (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config) {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _config = config;
    }


    /// <summary>
    /// Авторизация и выдача JWT токена
    /// </summary>
    public async Task<UserModel?> LoginAsync (LoginModel model) { 

        ApplicationUser? user = await _userManager.FindByEmailAsync (model.Email);

        if (user == null)
            return null;

        // попытка аутентификации пользователя 
        var result = await _signInManager.PasswordSignInAsync (user, model.Password, false, false);       

        if (!result.Succeeded)
            return null;

        // LOGER

        string token = GenerateJwtToken (user);

        IList<string> roles = await _userManager.GetRolesAsync (user);

        UserModel userModel = new UserModel { 
            Id = user.Id, 
            FullName = user.FullName, 
            Email = user.Email, 
            Token = token, 
            Roles = roles 
        };

        // LOGER

        return userModel;
    }


    /// <summary>
    /// Генерация JWT-токена
    /// </summary>
    private string GenerateJwtToken (ApplicationUser user) {

        byte[] key = Encoding.UTF8.GetBytes (_config["Jwt:Secret"]!);

        Claim[] claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        JwtSecurityToken token = new JwtSecurityToken (
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours (1),
            signingCredentials: new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256));

        // LOGER

        return new JwtSecurityTokenHandler ().WriteToken (token);
    }


    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    public async Task<IdentityResult> RegisterAsync (RegisterModel model) {

        ApplicationUser user = new ApplicationUser { 
            UserName = model.Email, 
            Email = model.Email, 
            FullName = model.FullName 
        };

        IdentityResult result = await _userManager.CreateAsync (user, model.Password);

        if (result.Succeeded) {
            if (!await _roleManager.RoleExistsAsync ("User"))
                await _roleManager.CreateAsync (new IdentityRole ("User"));

            await _userManager.AddToRoleAsync (user, "User");

            // LOGER
        }

        return result;
    }

    public async Task LogoutAsync () {
        await _signInManager.SignOutAsync ();

        // LOGER
    }

}
