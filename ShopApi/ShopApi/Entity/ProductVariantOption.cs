using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("ProductVariantOption")]
    public class ProductVariantOption
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ProductVariant")]
        public int ProductVariantId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        // Navigation Props
        public ProductVariant ProductVariant { get; set; }
        //public virtual ICollection<ProductSkuValue> ProductSkuValues { get; set; }

    }
}
