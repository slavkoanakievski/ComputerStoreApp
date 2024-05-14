using ComputerStoreApp.Models;
using ComputerStoreApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreApp.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly CoreDbContext _coreDbContext;

        public ProductCategoryRepository(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
        public async Task AddProductCategoriesAsync(List<ProductCategory> productCategories)
        {
            await _coreDbContext.ProductCategories.AddRangeAsync(productCategories);
            await _coreDbContext.SaveChangesAsync();
        }

        public async Task DeleteProductCategories(List<ProductCategory> productCategories)
        {
            _coreDbContext.ProductCategories.RemoveRange(productCategories);
            await _coreDbContext.SaveChangesAsync();
        }

        public async Task<ProductCategory> GetProductCategory(int productId, int categoryId)
        {
            return await _coreDbContext.ProductCategories
                .Where(pc => pc.ProductId == productId && pc.CategoryId == categoryId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoriesByProductIdAsync(int productId)
        {
            return await _coreDbContext.ProductCategories.Where(pc => pc.ProductId == productId).ToListAsync();
        }
    }
}
