using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLib;
public class PagedProductResponse {
    public List<ProductShortModel>? Items { get; set; }
    public int TotalCount { get; set; }
}
