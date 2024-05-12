using System.ComponentModel.DataAnnotations;

namespace ComputerStoreApp.Models.Resources
{
    public class CategoryResource
    {
        public int CategoryId { get; set; }
        [StringLength(300)]
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
    }
}
