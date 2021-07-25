using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("ProductOption")]
    [Index("OptionName", IsUnique = true)]
    public class ProductOption
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }

        [StringLength(50)]
        public string OptionName { get; set; }

        // Navigation props
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductSkuValue> ProductSkuValues { get; set; }
        public virtual ICollection<ProductOptionValue> ProductOptionValues { get; set; }

    }
}
