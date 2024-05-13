using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;

namespace ComputerStoreApp.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResource>> GetAllCategoriesAsync();
        Task<CategoryResource> GetCategoryAsync(int categoryId);
        Task<CategoryResource> AddCategoryAsync(CategoryDto categoryDto);
        Task<CategoryResource> EditCategoryAsync(int categoryId, CategoryDto category);
        Task<CategoryResource> DeleteCategoryAsync(int categoryId);
    }
}
