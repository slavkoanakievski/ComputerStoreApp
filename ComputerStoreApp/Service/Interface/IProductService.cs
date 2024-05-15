using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;

namespace ComputerStoreApp.Service.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResource>> GetAllProductsAsync();
        Task<ProductResource> GetProductAsync(int productId);
        Task<ProductResource> AddProductAsync(ProductDto productDto);
        Task<ProductResource> UpdateProductAsync(int productId, ProductDto productDto);
        Task<int> DeleteProductAsync(int productId);
        Task UpdateProductStock(List<StockInfoDto> stockInfoDtos);
        Task<CalculatedDiscountDto> CalculateDiscountForItems(List<PurchasedOrderItemDto> purchasedOrderItemDtos);
    }
}
