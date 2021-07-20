using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("ProductSku")]
    //[Index(IsUnique = true)]
    public class ProductSku
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public decimal Price { get; set; }

        [StringLength(45)]
        public string Sku { get; set; }

        // Navigation Props
        //public virtual ICollection<ProductSkuValue> ProductSkuValues { get; set; }
    }
}
