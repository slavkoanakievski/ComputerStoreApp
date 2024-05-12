using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreApp.Models
{
    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("ProductCategories")]
        public virtual Category Category { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductCategories")]
        public virtual Product Product { get; set; } = null!;
    }
}
