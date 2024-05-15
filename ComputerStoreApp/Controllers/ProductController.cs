using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;
using ComputerStoreApp.Service;
using ComputerStoreApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStoreApp.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IEnumerable<ProductResource>> GetAllProducts()
        {
            return await _productService.GetAllProductsAsync();
        }

        [HttpGet("product/{productId}")]
        public async Task<ProductResource> GetProductAsync(int productId)
        {
            return await _productService.GetProductAsync(productId);
        }

        [HttpPost("product/add")]
        public async Task<ProductResource> AddProductAsync([FromBody] ProductDto productDto)
        {
            return await _productService.AddProductAsync(productDto);
        }

        [HttpDelete("delete/product/{productId}")]
        public async Task<int> DeleteProduct(int productId)
        {
            return await _productService.DeleteProductAsync(productId);
        }

        [HttpPut("product/edit/{productId}")]
        public async Task<ProductResource> UpdateProductAsync(int productId, [FromBody] ProductDto productDto)
        {
            if (productId <= 0)
            {
                throw new Exception(message: "Invalid product id");
            }

            return await _productService.UpdateProductAsync(productId, productDto);
        }

        [HttpPost("product/update-stock")]
        public async Task<IActionResult> ProductStock([FromBody] List<StockInfoDto> stockInfoDtos)
        {
            try
            {
                await _productService.UpdateProductStock(stockInfoDtos);
                return Ok("Product stock imported successfully!");
            }
            catch
            {
                throw new Exception(message: "An error occurred while updating product stock");
            }
        }

        [HttpPost("calculate-discount-for-items")]
        public async Task<IActionResult> CalculateDiscountForItems([FromBody] List<PurchasedOrderItemDto> purchasedOrderItemDtos)
        {
            try
            {
                CalculatedDiscountDto calculatedDiscountDto = await _productService.CalculateDiscountForItems(purchasedOrderItemDtos);
                return Ok(calculatedDiscountDto);
            }
            catch
            {
                throw;
            }

        }
    }
}
