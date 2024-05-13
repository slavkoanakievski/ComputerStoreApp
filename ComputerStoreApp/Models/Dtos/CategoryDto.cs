using System.ComponentModel.DataAnnotations;

namespace ComputerStoreApp.Models.Dtos
{
    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
    }
}
