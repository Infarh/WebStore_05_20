using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Controllers;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BreadCrumbsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        private (BreadCrumbType Type, int Id, BreadCrumbType FromType) GetParameters()
        {
            var type = Request.Query.ContainsKey("SectionId")
                ? BreadCrumbType.Section
                : Request.Query.ContainsKey("BrandId")
                    ? BreadCrumbType.Brand
                    : BreadCrumbType.None;

            if ((string) ViewContext.RouteData.Values["action"] == nameof(CatalogController.Details))
                type = BreadCrumbType.Product;

            var id = 0;

            var from_type = BreadCrumbType.Section;

            switch (type)
            {
                default: throw new ArgumentOutOfRangeException();
                case BreadCrumbType.None: break;

                case BreadCrumbType.Section:
                    id = int.Parse(Request.Query["SectionId"].ToString());
                    break;

                case BreadCrumbType.Brand:
                    id = int.Parse(Request.Query["BrandId"].ToString());
                    break;

                case BreadCrumbType.Product:
                    id = int.Parse(ViewContext.RouteData.Values["id"].ToString() ?? string.Empty);
                    if (Request.Query.ContainsKey("FromBrand"))
                        from_type = BreadCrumbType.Brand;
                    break;
            }

            return (type, id, from_type);
        }


        public IViewComponentResult Invoke()
        {
            var (type, id, from_type) = GetParameters();

            switch (type)
            {
                default: return View(Enumerable.Empty<BreadCrumbViewModel>());

                case BreadCrumbType.Section:
                    return View(new []
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Section,
                            Id = id,
                            Name = _ProductData.GetSectionById(id).Name
                        }
                    });

                case BreadCrumbType.Brand:
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Brand,
                            Id = id,
                            Name = _ProductData.GetBrandById(id).Name
                        }
                    });

                case BreadCrumbType.Product:
                    var product = _ProductData.GetProductById(id);
                    return View(new []
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = from_type,
                            Id = from_type == BreadCrumbType.Section
                                ? product.Section.Id
                                : product.Brand.Id,
                            Name = from_type == BreadCrumbType.Section
                                ? product.Section.Name
                                : product.Brand.Name
                        },
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Product,
                            Id = product.Id,
                            Name = product.Name
                        }
                    });
            }
        }
    }
}
