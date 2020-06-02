using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly ILogger<CatalogController> _Logger;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, ILogger<CatalogController> Logger, IConfiguration Configuration)
        {
            _ProductData = ProductData;
            _Logger = Logger;
            _Configuration = Configuration;
        }

        public IActionResult Shop(int? SectionId, int? BrandId, int Page = 1)
        {
            var page_size = int.TryParse(_Configuration["PageSize"], out var size) ? size : (int?) null;

            _Logger.LogInformation("Запрошен каталог товаров для секции:{0} и бренда:{1}", 
                SectionId?.ToString() ?? "--", BrandId?.ToString() ?? "--");

            var filter = new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Page = Page,
                PageSize = page_size
            };
            var products = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Products = products.Products.FromDTO().Select(ProductMapping.ToView).OrderBy(p => p.Order),
                PageViewModel = new PageViewModel
                {
                    PageSize = page_size ?? 0,
                    PageNumber = Page,
                    TotalItems = products.TotalCount
                }
            });
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
            {
                _Logger.LogWarning("Запрошенный товар id:{0} не найден в каталоге!", id);
                return NotFound();
            }

            _Logger.LogInformation("Запрошена информация по товару:[{0}]{1}", product.Id, product.Name);


            return View(product.FromDTO().ToView());
        }
    }
}