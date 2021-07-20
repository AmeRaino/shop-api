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
    [Keyless]
    [Index( new [] { nameof(ProductVariantId), nameof(ProductSkuId) }, IsUnique = true)]
    [Index(new[] { nameof(ProductVariantOptionId), nameof(ProductVariantId), nameof(ProductId) }, IsUnique = true)]
    public class ProductSkuValue
    {
        [ForeignKey("ProductVariantOption")]
        public int? ProductVariantOptionId { get; set; }

        [ForeignKey("ProductSku")]
        public int? ProductSkuId { get; set; }

        [ForeignKey("ProductVariant")]
        public int? ProductVariantId { get; set; }

        [ForeignKey("Product")]
        public int? ProductId { get; set; }

        // Navigation Props
        public ProductVariantOption ProductVariantOption { get; set; }
        public ProductSku ProductSku { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
