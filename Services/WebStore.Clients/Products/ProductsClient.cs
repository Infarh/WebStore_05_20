using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration Configuration) : base(Configuration, WebAPI.Products) { }

        public IEnumerable<SectionDTO> GetSections() => Get<IEnumerable<SectionDTO>>($"{_ServiceAddress}/sections");

        public SectionDTO GetSectionById(int id) => Get<SectionDTO>($"{_ServiceAddress}/sections/{id}");

        public IEnumerable<BrandDTO> GetBrands() => Get<IEnumerable<BrandDTO>>($"{_ServiceAddress}/brands");

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{_ServiceAddress}/brands/{id}");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null) =>
            Post(_ServiceAddress, Filter ?? new ProductFilter())
               .Content
               .ReadAsAsync<IEnumerable<ProductDTO>>()
               .Result;

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");
    }
}
