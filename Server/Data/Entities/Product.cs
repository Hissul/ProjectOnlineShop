namespace Server.Data.Entities;

public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public byte[] Image { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ProductInfo ProductInfo { get; set; } = null!;
}
