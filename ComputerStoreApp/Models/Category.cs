using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreApp.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }

        [Key]
        public int CategoryId { get; set; }
        [StringLength(300)]
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }

        [InverseProperty(nameof(ProductCategory.Category))]
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Category other = (Category)obj;
            return CategoryId == other.CategoryId && CategoryName == other.CategoryName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CategoryId, CategoryName);
        }
    }
}
