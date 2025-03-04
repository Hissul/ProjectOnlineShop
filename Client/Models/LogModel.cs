using System.ComponentModel.DataAnnotations;

namespace Client.Models;

public class LogModel {
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [DataType (DataType.Password)]
    public string Password { get; set; } = default!;
}
