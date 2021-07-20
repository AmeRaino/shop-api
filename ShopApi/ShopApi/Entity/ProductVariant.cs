using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("ProductVariant")]
    //[Index(IsUnique = true)]
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        // Navigation Props
        public Product Product{ get; set; }
        public virtual ICollection<ProductVariantOption> ProductVariantOptions { get; set; }
        //public virtual ICollection<ProductSkuValue> ProductSkuValues { get; set; }
    }
}
