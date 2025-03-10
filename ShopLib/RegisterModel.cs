using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib;
public class RegisterModel : LoginModel {  
    [Required]
    [DataType (DataType.Password)]
    [Compare ("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; } = default!;

    [Required]
    public string FullName { get; set; }
}
