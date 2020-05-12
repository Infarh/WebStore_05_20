using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Index(/*[FromServices] IProductData Products*/) => View(_ProductData.GetProducts());

        public IActionResult Edit(int? id)
        {
            var product = id is null ? new Product() : _ProductData.GetProductById((int) id);

            if (product is null)
                return NotFound();

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName(nameof(Delete))]
        public IActionResult DeleteConfirm(int id) => RedirectToAction(nameof(Index));
    }
}