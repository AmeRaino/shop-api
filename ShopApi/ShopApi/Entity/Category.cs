using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entity
{
    [Table("Category")]
    [Index("Name", IsUnique = true)]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [MaxLength(50)]

        public string Name { get; set; }

        //Navigation props
        [ForeignKey("ParentId")]
        public Category Parent { get; set; }

        [InverseProperty(nameof(Category.Parent))]
        public virtual ICollection<Category> Children { get; set; }
        [InverseProperty(nameof(Product.Category))]
        public virtual ICollection<Product> Products { get; set; }

    }
}
