using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComputerStoreApp.Models.Dtos
{
    public class ProductDto
    {
        [StringLength(300)]
        public string ProductName { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public int? ProductStock { get; set; }
        public IEnumerable<int> Categories { get; set; }
    }
}
