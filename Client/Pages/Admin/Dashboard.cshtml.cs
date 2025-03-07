using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace Client.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public DashboardModel (IHttpContextAccessor httpContextAccessor) {
            HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            string? tmp = HttpContextAccessor.HttpContext.Session.GetString ("auth_token");
           

            if (tmp == null) {
                return RedirectToPage ("/Auth/Login");
            }

            string? role = HttpContextAccessor.HttpContext.Session.GetString ("user_role");

            if (role == "Admin") {                
                return Page ();
            }
           
            return RedirectToPage ("/Index");
        }


    }
}
