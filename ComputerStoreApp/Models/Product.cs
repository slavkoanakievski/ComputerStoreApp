using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreApp.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }

        [Key]
        public int ProductId { get; set; }
        [StringLength(300)]
        public string ProductName { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public int? ProductStock { get; set; }

        [InverseProperty(nameof(ProductCategory.Product))]
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Product otherProduct = (Product)obj;
            return ProductId == otherProduct.ProductId;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
    }
}
