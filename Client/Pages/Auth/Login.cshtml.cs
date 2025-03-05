using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Client.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ApiService _apiService;

        public LoginModel (ApiService apiService) {
            _apiService = apiService;
        }   

        [BindProperty]
        public LogModel LogModel { get; set; }

        public void OnGet() {}


        public async Task<IActionResult> OnPost() {


            UserModel? authResponse = await _apiService.LoginAsync (LogModel.Email, LogModel.Password);

            if (authResponse == null) {
                ModelState.AddModelError ("", "Ошибка входа. Проверьте email и пароль.");
                return Page ();
            }          

            // Сохраняем токен и роли в сессию
            HttpContext.Session.SetString ("auth_token", authResponse.Token);
            HttpContext.Session.SetString ("user_role", string.Join (",", authResponse.Roles));
           

            return RedirectToPage ("/Index");
        }


    }
}
