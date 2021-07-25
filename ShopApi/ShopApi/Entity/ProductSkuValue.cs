using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("ProductSkuValue")]
    
    public class ProductSkuValue
    {
        public int ProductId { get; set; }
        public int? SkuId { get; set; }
        public int OptionId { get; set; }
        public int ValueId { get; set; }

        public virtual ProductSku ProductSku { get; set; }
        public virtual ProductOption ProductOption { get; set; }
        public virtual ProductOptionValue ProductOptionValue { get; set; }

    }
}
