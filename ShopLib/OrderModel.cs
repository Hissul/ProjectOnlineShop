using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib;
public class OrderModel {
    public int Id { get; set; }
    public DateTime OrderedDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }

    public ICollection<OrderItemModel> ItemModels { get; set; } = 
        [];

    public static implicit operator List<object> (OrderModel? v) {
        throw new NotImplementedException ();
    }
}
