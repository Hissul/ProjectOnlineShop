using Microsoft.AspNetCore.SignalR.Protocol;

namespace Server.Data.Entities;

public class ProductInfo {
    public int Id { get; set; }
    public string Technique { get; set; }
    public string Material { get; set; }
    public string Plot { get; set; }
    public string Style { get; set; }
    public int Height { get; set; }
    public int Wight { get; set; }    
    public int Year { get; set; }
   

    public int ProductId { get; set; }
    public Product Product { get; set; } = null;
}
