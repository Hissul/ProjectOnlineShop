using Client.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopLib;

namespace Client.Pages.Admin.Users
{
    public class EditModel : PageModel
    {
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditModel (UserService userService, IHttpContextAccessor contextAccessor) {
            _userService = userService;
            _contextAccessor = contextAccessor;
        }


        [BindProperty]
        public UserModel User { get; set; }

        [BindProperty]
        public string UserRole { get; set; }

        [BindProperty]
        public List<SelectListItem> RoleItems { get; set; } = new ();

        [BindProperty]
        public string ReturnUrl { get; set; }

        [TempData]
        public string? Notification { get; set; }

        [TempData]
        public string? NotificationType { get; set; }



        public async Task<IActionResult> OnGet(string id, string returnUrl) {

            TempData.Remove ("Notification");
            TempData.Remove ("NotificationType");

            string? userId = _contextAccessor.HttpContext.Session.GetString("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            User = await _userService.GetUserAsync (id);

            UserRole = User.Roles[0];
            RoleItems = User.AllRoles.Select (r => new SelectListItem { Text = r, Value = r }).ToList ();

            ReturnUrl = returnUrl;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync () {

            User.Roles.Add (UserRole);

            bool result = await _userService.EditUserAsync (User);
           
            if (result) {
                Notification = "Пользователь успешно изменен.";
                NotificationType = "success";
                return RedirectToPage (ReturnUrl);
            }
           
             Notification = "Ошибка при изменении пользователя.";
             NotificationType = "error";
           
            return Page();
        }


    }
}
