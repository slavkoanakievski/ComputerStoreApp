using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComputerStoreApp.Models.Resources
{
    public class ProductResource
    {
        public int ProductId { get; set; }
        [StringLength(300)]
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public int? ProductStock { get; set; }
        public List<CategoryResource> Categories { get; set; }
    }
}
