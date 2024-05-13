using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Resources;

namespace ComputerStoreApp.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(int categoryId);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> EditCategoryAsync(Category category);
        Task<Category> DeleteCategoryAsync(int categoryId);
    }
}
