using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary>Секция товаров</summary>
    //[Table("Setctions")]
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        /// <summary>Идентификатор родительской секции</summary>
        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual Section ParentSection { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
