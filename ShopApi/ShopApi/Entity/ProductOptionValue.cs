using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("ProductOptionValue")]
    [Index("ValueName", IsUnique = true)]
    public class ProductOptionValue
    {
        public int ProductId { get; set; }

        public int ValueId { get; set; }
        public int OptionId { get; set; }
        [StringLength(50)]
        public string ValueName { get; set; }

        // Navigation props
        public virtual ProductOption ProductOption { get; set; }
        public virtual ICollection<ProductSkuValue> ProductSkuValues { get; set; }
    }
}
