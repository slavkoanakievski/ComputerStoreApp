using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Resources;
using ComputerStoreApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CoreDbContext _coreDbContext;
        public CategoryRepository(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;   
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _coreDbContext.Categories.ToListAsync();
        }
    }
}
