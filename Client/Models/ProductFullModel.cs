namespace Client.Models;

public class ProductFullModel {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public byte[] Image { get; set; }

    public string Technique { get; set; }
    public string Material { get; set; }
    public string Plot { get; set; }
    public string Style { get; set; }
    public string Size { get; set; }
    public int Year { get; set; }
}
