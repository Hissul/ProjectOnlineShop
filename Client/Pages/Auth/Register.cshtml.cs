
using ShopLib;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Client.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly LoginService _apiService;

        public RegisterModel (LoginService apiService) {
            _apiService = apiService;
        }

        [BindProperty]
        public ShopLib.RegisterModel RegModel { get; set; }


        public void OnGet(){}


        public async Task<IActionResult> OnPost () {

            bool success = await _apiService.RegisterAsync (RegModel.Email, RegModel.Password, RegModel.FullName);

            if (!success) {
                ModelState.AddModelError ("", "Ошибка регистрации. Попробуйте снова.");
                return Page ();
            }

            return RedirectToPage ("/Index");
        }


    }
}
