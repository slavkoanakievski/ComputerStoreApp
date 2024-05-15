using ComputerStoreApp.Models;
using ComputerStoreApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreApp.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly CoreDbContext _coreDbContext;
        public ProductRepository(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _coreDbContext.Products
                .Include(p => p.ProductCategories)
                .ToListAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            try
            {
                return await _coreDbContext.Products
                   .Where(p => p.ProductId == productId)
                    .Include(p => p.ProductCategories)
                   .FirstOrDefaultAsync();
            }
            catch
            {
                throw new Exception(message: "There is no product with id: " + productId);
            }
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _coreDbContext.Products.AddAsync(product);
            await _coreDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProductAsync(int productId)
        {
            Product product = GetProductAsync(productId).Result;
            if (product == null)
            {
                throw new ArgumentException(message: $"Product with id: {productId} not found!");
            }

            _coreDbContext.Products.Remove(product);
            await _coreDbContext.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _coreDbContext.Update(product);
            await _coreDbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductByNameAsync(string productName)
        {
            Product product = await _coreDbContext.Products
               .Where(p => p.ProductName == productName)
                .Include(p => p.ProductCategories)
               .FirstOrDefaultAsync();

            if (product != null)
            {
                return product;
            }
            else
            {
                return null;
            }
        }

        public async Task<Dictionary<int, int>> GetProductStocksAsync(List<int> productIds)
        {
            var productStocks = await _coreDbContext.Set<Product>()
              .Where(p => productIds.Contains(p.ProductId))
              .ToDictionaryAsync(p => p.ProductId, p => p.ProductStock ?? 0);

            return productStocks;
        }
    }
}
