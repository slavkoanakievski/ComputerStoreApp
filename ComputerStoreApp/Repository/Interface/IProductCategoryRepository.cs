using ComputerStoreApp.Models;

namespace ComputerStoreApp.Repository.Interface
{
    public interface IProductCategoryRepository
    {
        public Task AddProductCategoriesAsync(List<ProductCategory> productCategories);
        public Task DeleteProductCategories(List<ProductCategory> productCategories);

        public Task<ProductCategory> GetProductCategory(int productId, int categoryId);
        public Task<List<ProductCategory>> GetProductCategoriesByProductIdAsync(int productId);
    }
}
