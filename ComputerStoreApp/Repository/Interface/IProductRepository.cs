using ComputerStoreApp.Models;

namespace ComputerStoreApp.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int productId);
        Task<Product> GetProductByNameAsync(string productName);
        Task<Product> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task<Product> DeleteProductAsync(int productId);
        Task<Dictionary<int, int>> GetProductStocksAsync(List<int> productIds);
    }
}
