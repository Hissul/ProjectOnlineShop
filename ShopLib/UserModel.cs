using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib;
public class UserModel {
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public IList<string> Roles { get; set; } = [];
}
