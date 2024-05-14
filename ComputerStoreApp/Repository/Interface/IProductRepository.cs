using ComputerStoreApp.Models;

namespace ComputerStoreApp.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int productId);
        Task<Product> AddProductAsync(Product product);
        Task<Product> DeleteProductAsync(int productId);
    }
}
