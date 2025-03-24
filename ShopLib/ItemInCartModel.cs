using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib;

public class ItemInCartModel {
    public int Id { get; set; }
    public ProductShortModel Product { get; set; }
    public int Quantity { get; set; }
}
