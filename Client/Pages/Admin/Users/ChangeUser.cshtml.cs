using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopLib;

namespace Client.Pages.Admin.Users
{
    public class ChangeUserModel : PageModel
    {
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ChangeUserModel (UserService userService, IHttpContextAccessor contextAccessor) {
            _userService = userService;
            _contextAccessor = contextAccessor;
        }

        [BindProperty]
        public List<UserModel>? Users { get; set; }


        public async Task<IActionResult> OnGetAsync(){
            string? userId = _contextAccessor.HttpContext.Session.GetString ("user_id");

            if (userId == null) {
                return RedirectToPage ("/Auth/Login");
            }

            Users = await _userService.GetAllUsersAsync();
            return Page ();
        }
    }
}
