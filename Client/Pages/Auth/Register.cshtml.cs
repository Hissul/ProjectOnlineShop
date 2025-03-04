using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Client.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly ApiService _apiService;

        public RegisterModel (ApiService apiService) {
            _apiService = apiService;
        }

        [BindProperty]
        public RegModel RegModel { get; set; }

        public void OnGet(){}

        public async Task<IActionResult> OnPost () {

            string result = await _apiService.RegisterAsync (RegModel.Email, RegModel.Password);

            if (result == null) {
                ModelState.AddModelError ("", "Ошибка регистрации");
                return Page();
            }

            return RedirectToPage ("/Index");
        }
    }
}
