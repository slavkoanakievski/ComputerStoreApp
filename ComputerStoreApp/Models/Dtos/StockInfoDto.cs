using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerStoreApp.Models.Dtos
{
    public class StockInfoDto
    {
        public string Name { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public IEnumerable<string>? CategoryNames { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
