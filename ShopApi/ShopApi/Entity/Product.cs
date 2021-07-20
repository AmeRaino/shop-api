using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        // Navigation Props
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
        public virtual ICollection<ProductSku> ProductSkues { get; set; }
    }
}
