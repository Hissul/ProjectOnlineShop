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
    private readonly ILogger<AuthService> _logger;

    public AuthService (
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config,
        ILogger<AuthService> logger) {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _config = config;
        _logger = logger;
    }


    /// <summary>
    /// Авторизация и выдача JWT токена
    /// </summary>
    public async Task<UserModel?> LoginAsync (LoginModel model) {

        ApplicationUser? user = await _userManager.FindByEmailAsync (model.Email);

        if (user == null) {
            _logger.LogWarning ("Попытка входа с несуществующим email: {Email}", model.Email);
            return null;
        }

        var result = await _signInManager.PasswordSignInAsync (user, model.Password, false, false);

        if (!result.Succeeded) {
            _logger.LogWarning ("Неудачная попытка входа: {Email}", model.Email);
            return null;
        }

        _logger.LogInformation ("Успешный вход: {Email}", model.Email);

        string token = await GenerateJwtToken (user);
        IList<string> roles = await _userManager.GetRolesAsync (user);

        return new UserModel {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token,
            Roles = roles
        };
    }


    /// <summary>
    /// Генерация JWT-токена
    /// </summary>
    private async Task<string> GenerateJwtToken (ApplicationUser user) {

        byte[] key = Encoding.UTF8.GetBytes (_config["Jwt:Secret"]!);

        IList<string> userRoles = await _userManager.GetRolesAsync (user);

        List<Claim> claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Добавляем роли
        foreach (var role in userRoles) {
            claims.Add (new Claim (ClaimTypes.Role, role));
        }

        JwtSecurityToken token = new JwtSecurityToken (
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours (1),
            signingCredentials: new SigningCredentials (
                new SymmetricSecurityKey (key),
                SecurityAlgorithms.HmacSha256));

        _logger.LogInformation ("JWT токен сгенерирован для пользователя: {Email}", user.Email);

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

            _logger.LogInformation ("Новый пользователь зарегистрирован: {Email}", model.Email);
        }
        else {
            _logger.LogWarning ("Ошибка регистрации пользователя: {Email}. Ошибки: {Errors}",
                model.Email, string.Join (", ", result.Errors.Select (e => e.Description)));
        }

        return result;
    }

    public async Task LogoutAsync () {
        await _signInManager.SignOutAsync ();

        _logger.LogInformation ("Пользователь вышел из системы.");
    }

}
