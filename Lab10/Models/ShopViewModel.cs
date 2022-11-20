using System.Collections.Generic;

namespace Lab10.Models
{
    public class ShopViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
}
