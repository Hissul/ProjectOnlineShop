using System.ComponentModel.DataAnnotations;

namespace Client.Models;

public class RegModel {
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [DataType (DataType.Password)]
    public string Password { get; set; } = default!;

    [Required]
    [DataType (DataType.Password)]
    [Compare ("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; } = default!;
}
