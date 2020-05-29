using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapping
    {
        public static SectionDTO ToDTO(this Section Section) => Section is null ? null : new SectionDTO
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        };

        public static Section FromDTO(this SectionDTO Section) => Section is null ? null : new Section
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        };

        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section> Sections) => Sections?.Select(ToDTO);
    }
}