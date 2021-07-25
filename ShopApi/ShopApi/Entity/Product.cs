using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("Product")]
    [Index("Name", IsUnique = true)]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }


        // Navigation props
        public virtual ICollection<ProductSku> ProductSkus { get; set; }
        public virtual ICollection<ProductOption> ProductOptions { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
