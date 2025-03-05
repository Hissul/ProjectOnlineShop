namespace Server.Models;

public class UserModel {
    public string Email { get; set; }   
    public string Token { get; set; }
    public IList<string> Roles { get; set; } = [];
}
