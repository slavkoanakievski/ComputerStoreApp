using ComputerStoreApp.Models.Resources;

namespace ComputerStoreApp.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResource>> GetAllCategoriesAsync();
    }
}
