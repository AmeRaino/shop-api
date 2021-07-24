﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("ProductSku")]
    public class ProductSku
    {
        public int ProductId { get; set; }
        public int SkuId { get; set; }

        [MaxLength(64)]
        public string Sku { get; set; }

        // Navigation props
        public Product Product { get; set; }
        public virtual ICollection<ProductSkuValue> ProductSkuValues { get; set; }
    }
}
