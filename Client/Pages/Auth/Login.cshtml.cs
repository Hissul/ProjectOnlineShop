using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ApiService _apiService;

        public LoginModel (ApiService apiService) {
            _apiService = apiService;
        }

        //[BindProperty]
        //public string Email { get; set; }

        //[BindProperty]
        //public string Password { get; set; }

        [BindProperty]
        public LogModel LogModel { get; set; }

        public void OnGet() {}


        public async Task<IActionResult> OnPost() {

            string token = await _apiService.LoginAsync(LogModel.Email, LogModel.Password);

            if(token == null) {
                ModelState.AddModelError ("", "Ошибка входа");
                return Page();
            }

            _apiService.SetToken(token);
            return RedirectToPage ("/Index");
        }
    }
}
