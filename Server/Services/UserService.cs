using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Data;
using ShopLib;

namespace Server.Services;

public class UserService {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserService> _logger;

    public UserService (
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UserService> logger) {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }


    /// <summary>
    /// Получение всех пользователей кроме главного админа
    /// </summary>
    public async Task<List<UserModel>> GetAllUsersAsync () {
        _logger.LogInformation ("Получение списка всех пользователей");
        List<UserModel> users = await _userManager.Users
            .Where (u => u.Email != "admin@example.com")
            .Select (u => new UserModel {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            })
            .ToListAsync ();

        return users;
    }


    /// <summary>
    /// Получение пользователя
    /// </summary>
    public async Task<UserModel> GetUserAsync (string id) {
        _logger.LogInformation ("Получение пользователя по ID: {UserId}", id);
        ApplicationUser? user = await _userManager.FindByIdAsync (id);
        if (user == null) {
            _logger.LogWarning ("Пользователь с ID {UserId} не найден", id);
            return null;
        }

        IList<string> roles = await _userManager.GetRolesAsync (user);
        List<string?> allRoles = await _roleManager.Roles
            .Select (r => r.Name)
            .ToListAsync ();

        return new UserModel {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles,
            AllRoles = allRoles
        };
    }


    /// <summary>
    /// Редактирование пользователя
    /// </summary>
    public async Task<bool> EditUserAsync (UserModel model) {
        _logger.LogInformation ("Редактирование пользователя ID: {UserId}", model.Id);
        ApplicationUser? user = await _userManager.FindByIdAsync (model.Id);

        if (user == null) {
            _logger.LogWarning ("Пользователь для редактирования не найден: {UserId}", model.Id);
            return false;
        }

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;

        IdentityResult result = await _userManager.UpdateAsync (user);
        if (!result.Succeeded) {
            _logger.LogError ("Ошибка обновления пользователя: {Errors}", string.Join (", ", result.Errors.Select (e => e.Description)));
            return false;
        }

        IList<string> userRoles = await _userManager.GetRolesAsync (user);
        result = await _userManager.RemoveFromRolesAsync (user, userRoles);

        if (!result.Succeeded) {
            _logger.LogError ("Ошибка удаления ролей у пользователя {UserId}", user.Id);
            return false;
        }

        result = await _userManager.AddToRolesAsync (user, model.Roles);
        if (!result.Succeeded) {
            _logger.LogError ("Ошибка добавления ролей пользователю {UserId}", user.Id);
            return false;
        }

        _logger.LogInformation ("Пользователь успешно обновлён: {UserId}", user.Id);
        return true;
    }


}
