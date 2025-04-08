using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib;
public class ProductFullModel : ProductShortModel {
    public string Technique { get; set; }
    public string Material { get; set; }
    public string Plot { get; set; }
    public string Style { get; set; }
    public string Size { get; set; }
    public int Year { get; set; }
    public int Wight { get; set; }
    public int Height { get; set; }
}
