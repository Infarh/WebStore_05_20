using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class CatalogViewModel
    {
        public int? BrandId { get; set; }

        public int? SectionId { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
