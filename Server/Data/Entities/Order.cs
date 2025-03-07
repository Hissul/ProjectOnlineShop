namespace Server.Data.Entities;

public class Order {
    public int Id { get; set; }
    public DateTime OrderedDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";

    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
