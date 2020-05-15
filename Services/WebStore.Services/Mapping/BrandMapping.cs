using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapping
    {
        public static BrandDTO ToDTO(this Brand Brand) => Brand is null ? null : new BrandDTO
        {
            Id = Brand.Id,
            Name = Brand.Name
        };

        public static Brand FromDTO(this BrandDTO Brand) => Brand is null ? null : new Brand
        {
            Id = Brand.Id,
            Name = Brand.Name
        };
    }
}