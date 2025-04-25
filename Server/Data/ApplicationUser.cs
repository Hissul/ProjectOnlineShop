using Microsoft.AspNetCore.Identity;
using Server.Data.Entities;

namespace Server.Data;

public class ApplicationUser : IdentityUser {
    public string FullName { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.Now;

    public ICollection<Order> Orders { get; set; } = [];
    public Cart Cart { get; set; } = null!;
}
