using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib;
public class LoginModel {
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [DataType (DataType.Password)]
    public string Password { get; set; } = default!;
}
