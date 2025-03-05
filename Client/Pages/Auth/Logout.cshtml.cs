using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Client.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly ApiService _apiService;

        public LogoutModel (ApiService apiService) {
            _apiService = apiService;
        }

        public void OnGet(){ }

        public async Task<IActionResult> OnPost () {

            bool result = await _apiService.LogoutAsync(true);

            if (result == false) {
                ModelState.AddModelError ("", "Ошибка выхода.");
                return Page ();
            }

            HttpContext.Session.Remove ("auth_token");
            HttpContext.Session.Remove ("user_role");

            return RedirectToPage ("/Index");
        }
    }
}
