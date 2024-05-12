using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Resources;

namespace ComputerStoreApp.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
