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

    public UserService (ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    /// <summary>
    /// Получение всех пользователей кроме главного админа
    /// </summary>
    public async Task<List<UserModel>> GetAllUsersAsync () {

        List<UserModel> users = await _userManager.Users
            .Where(u => u.Email != "admin@example.com")
            .Select(u => new UserModel {
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
        ApplicationUser? user = await _userManager.FindByIdAsync (id);

        if (user == null) return null;

        IList<string> roles = await _userManager.GetRolesAsync (user);
        List<string?> allRoles = await _roleManager.Roles
            .Select(r => r.Name)
            .ToListAsync ();

        UserModel userModel = new UserModel {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles,
            AllRoles = allRoles 
        };
        return userModel;
    }


    public async Task<bool> EditUserAsync (UserModel model) {
        ApplicationUser? user = await _userManager.FindByIdAsync (model.Id);

        if (user == null) return false;

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;

        IdentityResult result = await _userManager.UpdateAsync (user);

        if (!result.Succeeded) {
           return false;
        }


        IList<string> userRoles = await _userManager.GetRolesAsync(user);
        result = await  _userManager.RemoveFromRolesAsync (user, userRoles);

        if (!result.Succeeded) {
            return false;
        }

        result = await _userManager.AddToRolesAsync (user, model.Roles);

        if (!result.Succeeded) {
            return false;
        }
        return true;
    }

   



}
