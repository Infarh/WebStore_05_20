using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void Details_Returns_With_Correct_View()
        {
            // A-A-A = Arrange - Act - Assert

            #region Arrange

            const int expected_product_id = 1;
            const decimal expected_price = 10m;

            var expected_name = $"Product id {expected_product_id}";
            var expected_brand_name = $"Brand of product {expected_product_id}";

            var product_data_mock = new Mock<IProductData>();
            product_data_mock
               .Setup(p => p.GetProductById(It.IsAny<int>()))
               .Returns<int>(id => new ProductDTO
               {
                   Id = id,
                   Name = $"Product id {id}",
                   ImageUrl = $"Image_id_{id}.png",
                   Order = 1,
                   Price = expected_price,
                   Brand = new BrandDTO
                   {
                       Id = 1,
                       Name = $"Brand of product {id}"
                   },
                   Section = new SectionDTO
                   {
                       Id = 1,
                       Name = $"Section of product {id}"
                   }
               });

            var logger_mock = new Mock<ILogger<CatalogController>>();

            var controller = new CatalogController(product_data_mock.Object, logger_mock.Object);

            #endregion

            #region Act

            var result = controller.Details(expected_product_id);

            #endregion

            #region Assert

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);

            Assert.Equal(expected_product_id, model.Id);
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_price, model.Price);
            Assert.Equal(expected_brand_name, model.Brand); 

            #endregion
        }
    }
}
