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

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            try
            {
                return await _coreDbContext.Categories.Where(c => c.CategoryId == categoryId).FirstOrDefaultAsync();
            }
            catch
            {
                throw new Exception(message: "There is no category with id: " + categoryId);
            }
        }
        public async Task<Category> AddCategoryAsync(Category category)
        {
            await _coreDbContext.Categories.AddAsync(category);
            await _coreDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> EditCategoryAsync(Category category)
        {
            _coreDbContext.Update(category);
            await _coreDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> DeleteCategoryAsync(int categoryId)
        {
            Category category = GetCategoryAsync(categoryId).Result;
            if (category == null)
            {
                throw new ArgumentException(message: $"Category with id: {categoryId} not found!");
            }

            _coreDbContext.Categories.Remove(category);
            await _coreDbContext.SaveChangesAsync();
            return category;
        }
    }
}
